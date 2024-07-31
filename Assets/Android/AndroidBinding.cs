using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class AndroidBinding : MonoBehaviour
{
    private static AndroidBinding _instance;

    public static AndroidBinding Instance
    {
        get
        {
            //Debug.Log("Bridge: Instance get");
            if (_instance == null)
            {
                //Debug.Log("Bridge: Instance get if null");
                _instance = FindObjectOfType<AndroidBinding>();

                if (_instance == null)
                {
                    //Debug.Log("Bridge: Instance and existing object not found");
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<AndroidBinding>();
                    singletonObject.name = typeof(AndroidBinding).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            //Debug.Log("Bridge: Awake. Restoring");
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //Debug.Log("Bridge: Awake. Destroying");
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    AndroidJavaObject unityActivity;
    AndroidJavaObject bridge;
    
    [Serializable]
    public class FloatEvent : UnityEvent<float> { };
    public FloatEvent OnRawEMGPwrReceived;
    public FloatEvent OnBatteryVoltageReceived;

    [Serializable]
    public class Event : UnityEvent { };
    public Event SwitchToWaitForUserState;

    // private bool didPlayerCrashThisTrial = false;
    // public int trialNumber;
    private string playerLevel;
    private int playerNormalScore;
    private float playerxCoordinate;
    private float playeryCoordinate;
    private float pipexCoordinate;
    private float pipeyCoordinate;
    private float pipeWidth;
    private int playerBonusScore;
    private int playerLife;
    private float coinAltitude;
    private float spaceshipCommandSignalAtCurvePeakPointCrossing;
    private float curveAmplitude;
    private float curveWidth;
    private float curveSpeed;
    private float spaceshipCommandSignalAtCrash;
    private float emgBoostFactor;
    private float coinHeight;
    private float emgRaw;
    private float emgFiltered;
    private float emgMaxFiltered;
    private float emgMinFiltered;
    private float emgScaled;
    private float lpf;

    // device selection through Canvas(UI)
    private List<string> deviceList;
    private string selectedDevice;
    private string selectedEMGDevice;
    private int selectedEmgDeviceChannel;
    private double batteryVoltage;
    private string firmwareVersion;
    private string deviceToSave = "None";

    public static string _versionHash = null;

    public Text sensor;
    public GameObject bleDeviceMenu;
    public Dropdown dropdown;
    private float emgPwr;
    public static string GetVersionHash()
    {
        if (_versionHash == null)
        {
            _versionHash = "unknown";
            var gitHash = UnityEngine.Resources.Load<TextAsset>("GitHash");
            if (gitHash != null)
            {
                _versionHash = gitHash.text;
            }
        }
        return _versionHash;
    }

    void Start()
    {    
        //Debug.Log("Bridge: Start");
        ConnectService();
        selectedDevice = "No device is selected!";
    }

    void ConnectService() {
        //Debug.Log("Bridge: Connect Service");
        // AndroidJavaClass is the Unity representation of a generic instance of java.lang.Class.
        // AndroidJavaClass Constructor - public AndroidJavaClass(string className); - Specifies the Java class name (e.g. java.lang.String).
        // Construct an AndroidJavaClass from the class name. This essentially means  
        // locate the class type and allocate a java.lang.Class object of that particular type.
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        // public FieldType GetStatic(string fieldName);
        // Parameters: fieldName --> The name of the field (e.g. <i>int counter;</i> would have fieldName = "counter").
        // Get the value of a static field in an object type. The generic parameter determines the field type.
        unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        //  
        bridge = new AndroidJavaObject("org.sralab.emgimu.unity_bindings.Bridge"); //
        // Triggers android to connect to the service and also pass the callback
        // object so the android bridge can pass in updates

        if (bridge == null) {
            //Debug.Log("Bridge: ERROR WTF");
        } else {
            //Debug.Log("Bridge: SUCCESS");
        }

        //Debug.Log("Bridge: still in connectService");

        // Setup the parameters we want to send to our native plugin.
        object[] parameters = new object[2];
        parameters[0] = unityActivity;
        parameters[1] = new AndroidPluginCallback(this);
        if (bridge != null)
        {
            bridge.Call("connectService", parameters);
            //Debug.Log("Calling connectService in if statement");
        }
    }

    IEnumerator OnResumeCoroutineWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SendSelectedDeviceToAndroid(selectedDevice);

    }

    private void OnApplicationFocus(bool focus)
    {
        //Debug.Log("OnApplicationFocus(), focus = " + focus + " deviceToSave = " + deviceToSave);
        if (deviceToSave != "None")
        {
            if (focus)
            {
                SwitchToWaitForUserState.Invoke();
                ConnectService();
                StartCoroutine(OnResumeCoroutineWithDelay(0.1f)); // wait for 100 ms for sensor to connect to service
            }
            else
            {
                object[] parameters = new object[1];
                parameters[0] = unityActivity;
                //Debug.Log("Calls disconnectService in onApplicationFocus.");
                bridge.Call("disconnectService", parameters);
            }
        }

    }

    void OnApplicationQuit()
    {
        // Disconnect from service (this will also delete the callback
        // object handle)
        object[] parameters = new object[1];
        parameters[0] = unityActivity;
        if (bridge != null)
        {
            //Debug.Log("Calls disconnectService in onApplicationQuit.");
            bridge.Call("disconnectService", parameters);
        }
    }

    // public void Update()
    // {
    //     if (emgPwr > 0)
    //     {
    //         OnRawEMGPwrReceived.Invoke(emgPwr);
    //         OnBatteryVoltageReceived.Invoke((float) batteryVoltage);
    //         sensor.text = selectedDevice;
    //     }
    // }

    public void Update()
    {
        //Debug.Log("[Update] Update called");

        // Check if emgPwr is greater than 0
        if (emgPwr > 0)
        {
            //Debug.Log("[Update] emgPwr is greater than 0: " + emgPwr);

            // Checking if OnRawEMGPwrReceived is null
            if (OnRawEMGPwrReceived != null)
            {
                //Debug.Log("[Update] Invoking OnRawEMGPwrReceived");
                OnRawEMGPwrReceived.Invoke(emgPwr);
            }
            else
            {
                Debug.LogError("[Update] OnRawEMGPwrReceived is null");
            }

            // Checking if OnBatteryVoltageReceived is null
            if (OnBatteryVoltageReceived != null)
            {
                //Debug.Log("[Update] Invoking OnBatteryVoltageReceived");
                OnBatteryVoltageReceived.Invoke((float)batteryVoltage);
            }
            else
            {
                Debug.LogError("[Update] OnBatteryVoltageReceived is null");
            }

            // Checking if sensor is null
            if (sensor != null)
            {
                //Debug.Log("[Update] Updating sensor text");
                sensor.text = selectedDevice;
            }
            else
            {
                Debug.LogError("[Update] sensor is null");
            }
        }
        else
        {
            //Debug.Log("[Update] emgPwr is not greater than 0: " + emgPwr);
        }
    }


    void emgPwrUpdated(int power)
    {
        //Debug.Log("DEBUG LOG: emgPwrUpdated: " + power);
        emgPwr = power;
    }

    void batteryLifeUpdated(double voltage)
    {
        batteryVoltage = voltage;
    }

    void firmwareVersionUpdated(string version)
    {
        firmwareVersion = version;
    }

    void OnDeviceListReceived(string msg)
    {
        deviceList = CreateBleDeviceList(ParseDeviceListMsgAndClean(msg));
        //Debug.Log("DEBUG LOG: OnDeviceListReceived: " + deviceList);

        PopulateDropdown();
        // // Hack to run (Change this to a list that is populated with the devices found)
        // SendSelectedDeviceToAndroid(deviceList[0]);
        // bleDeviceMenu.SetActive(false);
    }

    public void PopulateDropdown()
    {
        //Debug.Log("DEBUG LOG: PopulateDropdown");
        SetDropdownList(dropdown, deviceList);
    }


    public void SetDropdownList(Dropdown dropdown, List<string> myDeviceList)
    {
        print("called SetDropDownList");
        //Debug.Log("DEBUG LOG: SetDropdownList ");
        List<string> items = new List<string>();
        if (myDeviceList == null)
        {
            items.Add("No Devices");
            //Debug.Log("DEBUG LOG: No devices found and 'No Devices' text added to list.");
        }
        else
        {
            items.Add(null);
            for (int i = 0; i < myDeviceList.Count; i++)
            {
                items.Add(myDeviceList[i]);
            }
        }
        dropdown.options.Clear();
        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }
        // dropdown.Show(); // tells the dropdown to open 
        DropdownItemSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        //Debug.Log("DropdownItemSelected is called");
        int index = dropdown.value;
        if (index != 0)
        {
            SendSelectedDeviceToAndroid(dropdown.options[index].text);
            bleDeviceMenu.SetActive(false);
        }
    }

    string[] ParseDeviceListMsgAndClean(string msg)
    {
        string[] tempList = msg.Split(',');
        for (int i = 0; i < tempList.Length; i++)
        {
            tempList[i] = CleanString(tempList[i]);
        }
        return tempList;
    }

    private string CleanString(string dirtyString)
    {
        string myString;
        if (dirtyString.StartsWith("["))
        {
            myString = dirtyString.Split('[')[1];
            if (myString.EndsWith("]"))
            {
                myString = myString.Split(']')[0];
            }
        }

        else if (dirtyString.StartsWith(" "))
        {
            myString = dirtyString.Split(' ')[1];
            if (myString.EndsWith("]"))
            {
                myString = myString.Split(']')[0];
            }
        }
        else
        {
            myString = dirtyString;
        }

        return myString;
    }

    private List<string> CreateBleDeviceList(string[] myArr)
    {
        List<string> myList = new List<string>();
        foreach (string item in myArr)
        {
            for (int i = 0; i < 2; i++)
            {
                myList.Add(item + " - ch-" + (i + 1));
                //Debug.Log("DEBUG LOG: myList.add");
            }
        }
        return myList;
    }

    class AndroidPluginCallback : AndroidJavaProxy
    {
        private AndroidBinding _ab;

        public AndroidPluginCallback(AndroidBinding ab) : base("org.sralab.emgimu.unity_bindings.PluginCallback")
        {
            _ab = ab;
        }

        public void onSuccess(string msg) {
            bool success = int.TryParse(msg, out int power); 
            if (success)
                _ab.emgPwrUpdated(power);
        }
        public void onBatteryLife(string msg)
        {
            bool success = double.TryParse(msg, out double voltage); 
            if (success)
                _ab.batteryLifeUpdated(voltage);
        }

        public void onFirmwareVersion(string msg)
        {
            _ab.firmwareVersionUpdated(msg);
        }

        public void onError(string errorMessage)
        {
            //Debug.Log("ENTER callback onError: " + errorMessage);
        }

        public void sendDeviceList(string msg)
        {
            _ab.OnDeviceListReceived(msg);
            //Debug.Log("Debug Log: sendDeviceList: " + msg);
        }
    }

    [Serializable]
    private struct TrialInfo
    {
        // birdgame specific fields
        // public int playerScore;
        public Time TrialEndTime;
        public String Timestamp;
        // public int trialNumber;
        // public int playerLife;
        public string playerLevel;
        public int playerNormalScore;
        public float playerxCoordinate;
        public float playeryCoordinate;
        public float pipexCoordinate;
        public float pipeyCoordinate;
        public float pipeWidth;
        // public int playerBonusScore;
        // public float coinAltitude;
        // public float coinHeight;
        // public float curveAmplitude;
        // public float curveWidth;
        // public float curveSpeed;
        // public bool didPlayerCrashThisTrial;
        // public float spaceshipCommandSignalAtCrash;
        // public float spaceshipCommandSignalAtCurvePeakPointCrossing;
        // public float emgBoostFactor;
        public float lpf;
        public float emgRaw;
        public float emgFiltered;
        public float emgMaxFiltered;
        public float emgMinFiltered;
        public float emgScaled;
        public String sensorMACAddress;
        public int sensorChannel;
        public double batteryVoltage;
        public String firmwareVersion;
        public String softwareVersionHash;
    }
//Define trial as the midpoint between two horizontal pipes.
    public void OnTrialCompleted() {
        TrialInfo trial = new TrialInfo();
        trial.Timestamp = DateTime.Now.ToString("O");
        // trial.trialNumber = AndroidBinding.Instance.trialNumber;
        // trial.playerLife = playerLife;
        trial.playerNormalScore = AndroidBinding.Instance.playerNormalScore;
        trial.playerLevel = AndroidBinding.Instance.playerLevel;
        // trial.playerBonusScore = playerBonusScore;
        // trial.coinAltitude = coinAltitude;
        // trial.coinHeight = coinHeight;
        // trial.curveAmplitude = curveAmplitude;
        // trial.curveWidth = curveWidth;
        // trial.curveSpeed = curveSpeed;
        // trial.didPlayerCrashThisTrial = didPlayerCrashThisTrial;
        // trial.spaceshipCommandSignalAtCrash = spaceshipCommandSignalAtCrash;
        // trial.emgBoostFactor = emgBoostFactor;
        // trial.spaceshipCommandSignalAtCurvePeakPointCrossing = spaceshipCommandSignalAtCurvePeakPointCrossing;
        trial.lpf = AndroidBinding.Instance.lpf;
        trial.emgRaw = AndroidBinding.Instance.emgRaw;
        trial.emgFiltered = AndroidBinding.Instance.emgFiltered;
        trial.emgMaxFiltered = AndroidBinding.Instance.emgMaxFiltered;
        trial.emgMinFiltered = AndroidBinding.Instance.emgMinFiltered;
        trial.emgScaled = AndroidBinding.Instance.emgScaled;
        trial.sensorMACAddress = AndroidBinding.Instance.selectedEMGDevice;
        trial.sensorChannel = AndroidBinding.Instance.selectedEmgDeviceChannel - 1; // to make the logs 0-indexed
        trial.batteryVoltage = AndroidBinding.Instance.batteryVoltage;
        trial.firmwareVersion = AndroidBinding.Instance.firmwareVersion;
        trial.softwareVersionHash = GetVersionHash();
        trial.playerxCoordinate = AndroidBinding.Instance.playerxCoordinate;
        trial.playeryCoordinate = AndroidBinding.Instance.playeryCoordinate;
        trial.pipexCoordinate = AndroidBinding.Instance.pipexCoordinate;
        trial.pipeyCoordinate = AndroidBinding.Instance.pipeyCoordinate;
        trial.pipeWidth = AndroidBinding.Instance.pipeWidth;

        AndroidBinding.Instance.LogTrial(JsonUtility.ToJson(trial));
    }

    private void LogTrial(string trial) {
        // bridge = new AndroidJavaObject("org.sralab.emgimu.unity_bindings.Bridge"); //hardcode
        //Debug.Log("LogTrial: " + trial);
        object[] parameters = { trial };
        if (bridge != null)
        {
            bridge.Call("logTrial", parameters);  // put some code here to fail gracefully
            //Debug.Log("Bridge: LogTrial in if statement: " + trial);
        } 
        else 
        {
            //Debug.Log("ERROR: Bridge is null");
            //Debug.Log("Bridge selected sensor: " + selectedEMGDevice);
        }
    }

    public void SendSelectedDeviceToAndroid(string selectedDeviceAndChannel)
    {

        object[] parameters = new object[1];
        selectedEMGDevice = selectedDeviceAndChannel.Substring(0, 17);
        var temp = selectedDeviceAndChannel.Substring(23);
        selectedEmgDeviceChannel = System.Convert.ToInt32(temp);
        SetSensorMac(selectedEMGDevice);
        SetSensorChannel(selectedEmgDeviceChannel);
        selectedDevice = selectedDeviceAndChannel;
        parameters[0] = selectedDevice;
        deviceToSave = selectedDevice;
        //Debug.Log("SendSelectedDeviceToAndroid Bridge: " + bridge);
        //Debug.Log("SendSelectedDeviceToAndroid selectedDevice: " + selectedEMGDevice);
        if (bridge != null)
        {
            bridge.Call("selectDevice", parameters);
        }
    }

    // public void SetTrialNumber(int number)
    // {
    //     AndroidBinding.Instance.trialNumber = number;
    //     //Debug.Log("SetTrialNumber Bridge: " + bridge);
    // }
    public void SetPlayerLevel(string currentLevel)
    {
        AndroidBinding.Instance.playerLevel = currentLevel;
    }
    public void SetPlayerNormalScore(int playerScore)
    {
        AndroidBinding.Instance.playerNormalScore = playerScore;
    }

    public void setPlayerXCoordinate(float xcoordinate)
    {
        AndroidBinding.Instance.playerxCoordinate = xcoordinate;
    }

    public void setPlayerYCoordinate(float ycoordinate)
    {
        AndroidBinding.Instance.playeryCoordinate = ycoordinate;
    }

    public void setPipeXCoordinate(float xcoordinate)
    {
        AndroidBinding.Instance.pipexCoordinate = xcoordinate;
    }

    public void setPipeYCoordinate(float ycoordinate)
    {
        AndroidBinding.Instance.pipeyCoordinate = ycoordinate;
    }

    public void setPipeWidth(float width)
    {
        AndroidBinding.Instance.pipeWidth = width;
    }

    // public void SetPlayerBonusScore(int bonusScore)
    // {
    //     playerBonusScore = bonusScore;
    // }

    // public void SetPlayerLife(int life)
    // {
    //     playerLife = life;
    // }

    // public void SetPlayerCrashTrialRecord(bool didCrash)
    // {
    //     didPlayerCrashThisTrial = didCrash;
    // }

    // public void SetPlayerAltitudeAtCrash(float crashAltitude)
    // {
    //     spaceshipCommandSignalAtCrash = crashAltitude;
    // }

    // public void SetCoinAltitude(float altitude)
    // {
    //     coinAltitude = altitude;
    // }

    // public void SetCoinPaddedHeight(float height)
    // {
    //     coinHeight = height;
    // }

    // public void SetSpaceshipCommandSignalAtCurvePeakPointCrossing(float verticalCommand)
    // {
    //     spaceshipCommandSignalAtCurvePeakPointCrossing = verticalCommand;
    // }

    // public void SetCurveAmplitude(float amplitude)
    // {
    //     curveAmplitude = amplitude;
    // }

    // public void SetCurveWidth(float width)
    // {
    //     curveWidth = width;
    // }

    // public void SetCurveSpeed(float speed)
    // {
    //     curveSpeed = speed;
    // }

    // public void SetEMGBoost(float boost)
    // {
    //     emgBoostFactor = boost;
    // }

    public void SetLPF(float LPF)
    {
        AndroidBinding.Instance.lpf = LPF;
    }

    public void SetRawEMG(float rawEMG)
    {
        AndroidBinding.Instance.emgRaw = rawEMG;
    }
    public void SetFilteredEMG(float filteredEMG)
    {
        AndroidBinding.Instance.emgFiltered = filteredEMG;
    }
    public void SetMaxFilteredEMG(float filteredMaxEMG)
    {
        //Fix so they all look like this.
        AndroidBinding.Instance.emgMaxFiltered = filteredMaxEMG;
    }
    public void SetMinFilteredEMG(float filteredMinEMG)
    {
        AndroidBinding.Instance.emgMinFiltered = filteredMinEMG;
    }

    public void SetScaledEMG(float scaledEMG)
    {
        AndroidBinding.Instance.emgScaled = scaledEMG;
    }

    private void SetSensorMac(String MAC)
    {
        AndroidBinding.Instance.selectedEMGDevice = MAC;
    }

    private void SetSensorChannel(int channel)
    {
        AndroidBinding.Instance.selectedEmgDeviceChannel = channel;
    }
}