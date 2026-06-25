/*
 * テキスト表示アニメーションとSE再生クラス
 */

using UnityEngine;
using TMPro;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    private TMP_Text uiText;
    private string fullText;
    private string currentText = "";
    public AudioSource typingSound;

    void Awake()
    {
        uiText = GetComponent<TMP_Text>();
        fullText = uiText.text;
    }

    void OnEnable()
    {
        StartCoroutine(ShowText());
    }

    public void SetText(string text)
    {
        fullText = text;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            uiText.text = currentText;
            typingSound.Play();
            yield return new WaitForSeconds(0.1f);
        }
    }
}