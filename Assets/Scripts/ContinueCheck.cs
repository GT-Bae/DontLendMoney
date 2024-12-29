using System;
using UnityEngine;
using UnityEngine.UI;

public class ContinueCheck : MonoBehaviour
{
    public Button yourButton;

    private void Start() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 0);
        if (currentDay > 0)
        {
            yourButton.GetComponent<Image>().color = Color.white; // Set button color to white
            yourButton.interactable = true; // Enable button click
        }
        else
        {
            yourButton.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f); // Set button color to light gray
            yourButton.interactable = false; // Disable button click
        }
    }
}
    
