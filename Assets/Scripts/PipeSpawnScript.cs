using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float initialSpawnRate = 3;
    private float spawnRate = 2;
    private float timer = 0;
    public float heightOffset = 2;
    public float minScale = 1f;
    public float maxScale = 10f;
    public Slider widthSlider; // Reference to the WidthSlider in the Unity UI

// Declare the UpdateAndroidTrialLog event
    // [Serializable]
    // public class Event : UnityEvent { };
    // public Event updateAndroidTrialLog; // Reference to the UpdateAndroidTrialLog event

    // [Serializable]
    // public class IntEvent : UnityEvent<int> { };
    // public IntEvent updateTrialInfoWithTrialNumber;
    // // private int trialNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        spawnPipe();
        // logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
            Debug.Log("Spawn Rate: " + spawnRate); // Print spawnRate to console

            // // Invoke the UpdateAndroidTrialLog event
            // updateAndroidTrialLog.Invoke();
            // trialNumber++;
            // updateTrialInfoWithTrialNumber.Invoke(trialNumber);
            // //Update trial number
            // AndroidBinding.Instance.SetTrialNumber(trialNumber);

        }
        
        maxScale = widthSlider.value; // Update maxScale based on the WidthSlider value
    }

    void spawnPipe()
    {
        float highestPoint = transform.position.y + heightOffset;
        float lowestPoint = transform.position.y - heightOffset;
        float randomWidth = UnityEngine.Random.Range(minScale, maxScale);

        GameObject newPipe = Instantiate(pipe, new Vector3(transform.position.x, UnityEngine.Random.Range(lowestPoint, highestPoint), 0), transform.rotation);

        Vector3 currentScale = newPipe.transform.localScale;
        newPipe.transform.localScale = new Vector3(randomWidth, currentScale.y, currentScale.z);

        spawnRate = initialSpawnRate + randomWidth;

        //Send pipe x and y coordinates to firestore
        AndroidBinding.Instance.setPipeWidth(newPipe.transform.localScale.x);
        AndroidBinding.Instance.setPipeXCoordinate(newPipe.transform.position.x);
        AndroidBinding.Instance.setPipeYCoordinate(newPipe.transform.position.y);
    }
}



