using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetName : MonoBehaviour
{
    public InputField inputField;
    public TMP_Text displayText;

    public void setName() {
        PlayerPrefs.SetString("Name", inputField.text);
        displayText.text = inputField.text;
    }
}