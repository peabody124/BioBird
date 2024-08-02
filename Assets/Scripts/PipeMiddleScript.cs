using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

public class PipeMiddleScript : MonoBehaviour
{
    public LogicScript logic;
    private Scene currentScene; // Declare currentScene as a class-level variable
    private BirdScript birdScript; // Reference to the BirdScript component

    private bool isResettingPosition = false; // Flag to check if the position is being reset
    private Vector3 resetStartPosition; // Starting position for resetting

    public float resetDuration = 1.0f; // Duration for the reset animation (you can adjust this)

    // Declare the UpdateAndroidTrialLog event
    [Serializable]
    public class Event : UnityEvent { };
    public Event updateAndroidTrialLog; // Reference to the UpdateAndroidTrialLog event

    [Serializable]
    public class IntEvent : UnityEvent<int> { };
    public IntEvent updateTrialInfoWithTrialNumber;
    // private int trialNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        birdScript = FindObjectOfType<BirdScript>(); // Get the BirdScript component

        // Get the currently active scene
        currentScene = SceneManager.GetActiveScene();

        // Print the name of the current scene
        Debug.Log("Current Scene: " + currentScene.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (logic.playerScore == 25 && currentScene.name == "Level1")
        {
            logic.levelTwo();
        }
        else if (logic.playerScore == 50 && currentScene.name == "Level2")
        {
            logic.levelThree();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (this.gameObject.name == "Middle")
        {
            Debug.Log("this.gameObject.name == " + this.gameObject.name);
            logic.addScore(1);

            // Check if bird's x position is between -4 and -9
            if (birdScript.transform.position.x >= -11f && birdScript.transform.position.x <= -7f)
            {
                // Start the reset position coroutine if it's not already running
                if (!isResettingPosition)
                {
                    resetStartPosition = birdScript.transform.position;
                    StartCoroutine(ResetBirdPosition());
                }
            }
        }

        if (this.gameObject.name == "LogTrial")
        {
            Debug.Log("this.gameObject.name == " + this.gameObject.name);
            // Invoke the UpdateAndroidTrialLog event
            updateAndroidTrialLog.Invoke();
            // trialNumber++;
            // updateTrialInfoWithTrialNumber.Invoke(trialNumber);
            // //Update trial number
            // AndroidBinding.Instance.SetTrialNumber(trialNumber);
            // Debug.Log("Trial Number: " + trialNumber);
        }
    }

    IEnumerator ResetBirdPosition()
    {
        isResettingPosition = true;
        float elapsedTime = 0f;

        while (elapsedTime < resetDuration)
        {
            birdScript.transform.position = Vector3.Lerp(resetStartPosition, Vector3.zero, elapsedTime / resetDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly (0, y, z)
        birdScript.transform.position = new Vector3(0f, birdScript.transform.position.y, birdScript.transform.position.z);

        isResettingPosition = false;
    }
}

