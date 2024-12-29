using UnityEngine;
using TMPro;

using System.Collections;

public class OpenTyping : MonoBehaviour
{
    private TMP_Text uiText;
    private string fullText;
    private string currentText = "";
    public AudioSource typingSound;

    void Awake()
    {
        uiText = GetComponent<TMP_Text>();
    }

    public void SetText(string text)
    {
        fullText = text;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        currentText = "";
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            uiText.text = currentText;
            typingSound.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
