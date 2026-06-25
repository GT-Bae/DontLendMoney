/* 
 * 就寝時の画面フェード（フェードイン・フェードアウト）を制御するクラス
 */

using System.Collections;
using UnityEngine;
using TMPro;

public class BedtoSleep : MonoBehaviour
{
    private GameEvent gameEvent; 
    public GameObject panel;
    public TextMeshProUGUI messageText;
    public GameObject sleepUI;
    public GameObject FirstsleepUI;

    void Start()
    {
        gameEvent = GetComponent<GameEvent>();
        if (!panel)
        {
            Debug.LogError("Panelオブジェクトが見つかりません。");
            throw new MissingComponentException();
        }

        if (!messageText)
        {
            Debug.LogError("MessageTextオブジェクトが見つかりません。");
            throw new MissingComponentException();
        }
    }

    public void FadeIn()
    {
        panel.SetActive(true);
        messageText.gameObject.SetActive(true);
        StartCoroutine(CoFadeIn());
    }

    public void FadeOut()
    {
        panel.SetActive(true);
        messageText.gameObject.SetActive(true);
        StartCoroutine(CoFadeOut());
    }

    public void FadeOutWithMessage()
    {
        int today = PlayerPrefs.GetInt("CurrentDay",1);
        messageText.text = today + "日";
        messageText.gameObject.SetActive(true);
        sleepUI.SetActive(false);
        FadeOut();
        StartCoroutine(WaitAndFadeIn(3f));
    }

    IEnumerator CoFadeIn()
    {   
        gameEvent.DecreaseDaysAndCheckEvents(); //返済期限の更新およびイベントチェック
        float elapsedTime = 0f;
        float fadedTime = 0.5f;

        while (elapsedTime <= fadedTime)
        {
            float alphaValue = Mathf.Lerp(1f, 0f, elapsedTime / fadedTime);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaValue);
            messageText.alpha = alphaValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // フェードイン終了後、不透明度を0に設定して非表示化
        panel.GetComponent<CanvasRenderer>().SetAlpha(0f);
        messageText.alpha = 0f;

        panel.SetActive(false);
        messageText.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f;
        float fadedTime = 0.5f;

        while (elapsedTime <= fadedTime)
        {
            float alphaValue = Mathf.Lerp(0f, 1f, elapsedTime / fadedTime);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaValue);
            messageText.alpha = alphaValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    IEnumerator WaitAndFadeIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (FirstsleepUI.activeSelf)
        {
            FirstsleepUI.SetActive(false);
            Debug.Log("FirstsleepUIを非表示にしました。");
        }
        FadeIn();
    }
}