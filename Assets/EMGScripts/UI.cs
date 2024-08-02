using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UI : MonoBehaviour
{
    private bool isMaxDecay = false;
    bool isMenu;
    bool pauseFlag;
    // bool isStartGame;
    float time = 0.0f;
    string niceTime;

    // public static float a;
    // public static float sigma;
    // public static float muStep;
    // public static float lastMuStep;
    // string state;

    public GameObject menu;
    public GameObject calibrationPanel;
    public GameObject startGameObject;
    public GameObject pauseButtonGameObject;
    public Text Timer;
    // public Text playerLife;
    public Text normalPlayerScore;
    // public Text bonusPlayerScore;

    // public Text speedToDisplay;
    // public Text amplitudeToDisplay;
    // public Text sigmaToDisplay;
    public Text lastMaxEmgToDisplay;
    public Text commandSignalToDisplay;
    // public Text probabilityToDisplay;

    public Text rawEMG;
    public Text filteredEMG;
    public Text maxFiltered;
    public Text minFiltered;
    public Text emgScaledText;
    public Text lpfDisplay;
    public Text boostText;
    public Text maxDecayToDisplay;
    public Text maxDecayOnOffButton;
    public Text batteryVoltageText;
    public Text isActiveMaxTrialBased;
    // public Text coinAltitude;

    public Sprite calibratingBG;
    public Sprite waitingBG;
    public Color color = Color.white;

    [Serializable]
    public class Event : UnityEvent { };
    public Event StartGameEvent;

    [Serializable]
    public class FloatEvent : UnityEvent<float> { };
    public FloatEvent SetMaxDecay;

    // private string currentTime;
    // public Text currentTimeToDisplay;
    public Text sensorMacAndChannelToDisplay;

    void Start()
    {
        // position 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;          // Disable screen diming
        pauseFlag = false;
        isMenu = true;
        // isStartGame = false;
        pauseButtonGameObject.SetActive(false);
        startGameObject.SetActive(false);
        ToggleMaxDecay();
    }

    void FixedUpdate()
    {
        // if (isStartGame)
        // {
        //     GameTimer();
        // }
        // currentTime = DateTime.Now.ToString("h:mm tt");
        // currentTimeToDisplay.text = currentTime;
    }

    public void CallExitButton()
    {
        Debug.Log("Exit Button Pressed!");
        Application.Quit();
    }

    public void Pause()
    {
        pauseFlag = !pauseFlag;
        if (pauseFlag)
        {
            Time.timeScale = 0;
            // MuteAllSound();
        }
        else
        {
            Time.timeScale = 1;
            // UnMuteAllSound();
        }
    }

    public void ToggleMenu()
    {
        isMenu = !isMenu;
        menu.SetActive(isMenu);
    }

    // public void MuteAllSound()
    // {
    //     AudioListener.volume = 0;
    // }

    // public void UnMuteAllSound()
    // {
    //     AudioListener.volume = 1;
    // }

    public void GameTimer()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        Timer.text = niceTime;
    }

    public void CalibrationComplete()
    {
        //calibrationPanel.GetComponent<Image>().color = new Color32(16, 255, 179, 159); // new Color32(43, 229, 33, 159); green color

        calibrationPanel.GetComponent<Image>().sprite = waitingBG;
        calibrationPanel.GetComponent<Image>().color = new Color32(16, 255, 179, 159); // new Color32(43, 229, 33, 159); green color

    }
    public void CalibrationPanelOn()
    {
        calibrationPanel.SetActive(true);
        //calibrationPanel.GetComponent<Image>().color = new Color32(43, 33, 240, 159); // blue color 
        calibrationPanel.GetComponent<Image>().sprite = calibratingBG; // blue color
        color.a = 0.5f;
        calibrationPanel.GetComponent<Image>().color = color;
        //gameObject.GetComponent<Image>().sprite = FULLHP;
    }

    public void StartGameButton()
    {
        startGameObject.SetActive(false);
        SetPauseButtonActive();
        //calibrationPanel.SetActive(false);
        StartGameEvent.Invoke();
        // isStartGame = true;
    }


    public void SetActiveStartGameButton()
    {
        startGameObject.SetActive(true);
    }

    public void SetPauseButtonActive()
    {
        pauseButtonGameObject.SetActive(true);
    }

    // public void SetPlayerLife(int life)
    // {
    //     playerLife.text = life.ToString() + "%";
    // }

    public void SetPlayerNormalScore(int normalScore)
    {
        normalPlayerScore.text = normalScore.ToString();
    }

    // public void SetPlayerBonusScore(int bonusScore)
    // {
    //     bonusPlayerScore.text = bonusScore.ToString();
    // }

    public void EmgRawSignalObserver(float emgRaw)
    {
        rawEMG.text = emgRaw.ToString("N");
    }

    public void EmgFilteredSignalObserver(float emgFiltered)
    {
        filteredEMG.text = emgFiltered.ToString("N");
    }

    public void EmgFilteredMinSignalObserver(float emgFilteredMin)
    {
        minFiltered.text = emgFilteredMin.ToString("N");
    }

    public void EmgFilteredMaxSignalObserver(float emgFilteredMax)
    {
        maxFiltered.text = emgFilteredMax.ToString("N");
    }

    public void LastEmgFilteredMaxObserver(float lastEmgFilteredMax)
    {
        lastMaxEmgToDisplay.text = lastEmgFilteredMax.ToString("N");
    }

    public void EmgScaledSignalObserver(float emgScaled)
    {
        emgScaledText.text = emgScaled.ToString("N");
    }

    public void MaxDecayFactorEmgSignalObserver(float maxDecay)
    {
        maxDecayToDisplay.text = maxDecay.ToString();
    }

    public void BoostFactorEmgSignalObserver(float boost)
    {
        boostText.text = boost.ToString("F1");
    }

    public void LpfEmgSignalObserver(float lpf)
    {
        lpfDisplay.text = lpf.ToString("N");
    }

    // public void CurveAmplitudeObserver(float amplitude)
    // {
    //     amplitudeToDisplay.text = amplitude.ToString("N");
    // }

    // public void CurveSpeedObserver(float muStep)
    // {
    //     speedToDisplay.text = muStep.ToString("F3");
    // }

    // public void CurveWidthObserver(float sigma)
    // {
    //     sigmaToDisplay.text = sigma.ToString("N");
    // }

    // public void MaxCurveAmplitudeProbabilityObserver(float probability)
    // {
    //     probabilityToDisplay.text = probability.ToString();
    // }

    // public void SpaceshipScreenScaledCommandSignalObserver(float scaledCommand)
    // {
    //     commandSignalToDisplay.text = scaledCommand.ToString("N");
    // }

    // public void CoinAltitudeObserver(float height)
    // {
    //     coinAltitude.text = height.ToString("F2");
    // }

    public void ToggleMaxDecay()
    {
        isMaxDecay = !isMaxDecay;
        if (isMaxDecay)
        {
            SetMaxDecay.Invoke(1.0f);
            maxDecayOnOffButton.color = Color.red;
            maxDecayOnOffButton.text = "OFF";
        }
        else
        {
            SetMaxDecay.Invoke(0.9999f);
            maxDecayOnOffButton.color = Color.green;
            maxDecayOnOffButton.text = "ON";
        }
    }

    public void SetBatteryVoltage(float voltage)
    {
        batteryVoltageText.text = voltage.ToString("N") + "V";
    }

    public void SetCalibrationPanelGreenColor()
    {
        Color32 greenColor = new Color32(43, 229, 33, 159);
        calibrationPanel.GetComponent<Image>().color = greenColor;
    }

    public void DisableCalibrationPanel()
    {
        calibrationPanel.SetActive(false);
    }

    public void SetActiveMaxTrialBasedStatus(bool status)
    {
        isActiveMaxTrialBased.text = status.ToString();
    }
}
