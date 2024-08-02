using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private Slider slider;
    public SignalProcessing signalProcessingScript; // Reference to the SignalProcessing script

    private void Start()
    {
        slider = GetComponent<Slider>();
        Debug.Log("SliderController Start");

        // Subscribe to the EmgScaledValueChanged event
        SignalProcessingReference.signalProcessingScript.EmgScaledValueChanged += UpdateSliderValue;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the EmgScaledValueChanged event when the script is destroyed
        signalProcessingScript.EmgScaledValueChanged -= UpdateSliderValue;
        Debug.Log("SliderController OnDestroy");
    }

    private void UpdateSliderValue(float value)
    {
        slider.value = value;
        Debug.Log("SliderController UpdateSliderValue: " + value);
    }
}




