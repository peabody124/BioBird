using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -45;

    private BirdScript birdScript;

    void Start()
    {
        birdScript = FindObjectOfType<BirdScript>();
    }

    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed * Time.deltaTime);
        Debug.Log("Bird Position " + birdScript.transform.position.x);
        if (birdScript.birdIsAlive != false && birdScript.transform.position.x <= -10)
        {
            birdScript.TriggerGameOver();
        }
        
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}

