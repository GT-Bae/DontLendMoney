using UnityEngine;
using TMPro;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    private TMP_Text uiText;
    public string fullText;
    private string currentText = "";

    void Awake()
    {
        uiText = GetComponent<TMP_Text>();
        fullText = uiText.text;
    }

    void OnEnable()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            uiText.text = currentText;
            yield return new WaitForSeconds(0.1f);
        }
    }
}