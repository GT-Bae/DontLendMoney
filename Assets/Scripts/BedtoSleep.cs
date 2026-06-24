/* 
 * BedtoSleep 클래스는 패널과 텍스트의 불투명도를 조절하여 페이드 인/아웃 효과를 구현합니다.
 * 패널과 텍스트의 알파 값을 조절하여 페이드 인/아웃을 수행합니다.
 * FadeOutWithMessage 함수는 메시지를 화면에 표시하고 5초 후에 페이드 인을 실행합니다.
 */

using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class BedtoSleep : MonoBehaviour // Panel 불투명도 조절해 페이드인 or 페이드아웃
{
    private GameEvent gameEvent; 
    public GameObject panel; // 불투명도를 조절할 Panel 오브젝트
    public TextMeshProUGUI messageText; // 화면에 표시할 텍스트 UI 요소
    public GameObject sleepUI;
    public GameObject FirstsleepUI;
    private Action onCompleteCallback; // FadeIn 또는 FadeOut 다음에 진행할 함수

    void Start()
    {
        gameEvent = GetComponent<GameEvent>();
        if (!panel)
        {
            Debug.LogError("Panel 오브젝트를 찾을 수 없습니다.");
            throw new MissingComponentException();
        }

        if (!messageText)
        {
            Debug.LogError("MessageText 오브젝트를 찾을 수 없습니다.");
            throw new MissingComponentException();
        }
    }

    public void FadeIn()
    {
        panel.SetActive(true); // Panel 활성화
        messageText.gameObject.SetActive(true); // 메시지 텍스트 활성화
        StartCoroutine(CoFadeIn()); //페이드인 시작
    }

    public void FadeOut()
    {
        panel.SetActive(true); // Panel 활성화
        messageText.gameObject.SetActive(true); // 메시지 텍스트 활성화
        StartCoroutine(CoFadeOut()); //페이드아웃 시작
    }

    public void FadeOutWithMessage()
    {
        int today = PlayerPrefs.GetInt("CurrentDay",1);
        messageText.text = today + "일"; // 메시지 설정
        messageText.gameObject.SetActive(true); // 메시지 텍스트 활성화
        sleepUI.SetActive(false); // sleepUI 비활성화
        FadeOut();
        StartCoroutine(WaitAndFadeIn(3f)); // 3초 후에 FadeIn 호출
    }

    IEnumerator CoFadeIn()
    {   
        gameEvent.DecreaseDaysAndCheckEvents(); // 갚는거 리스트
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 0.5f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            float alphaValue = Mathf.Lerp(1f, 0f, elapsedTime / fadedTime);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaValue);
            messageText.alpha = alphaValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // FadeIn 끝나고 완전히 불투명도 0으로 설정
        panel.GetComponent<CanvasRenderer>().SetAlpha(0f);
        messageText.alpha = 0f;

        panel.SetActive(false); // Panel을 비활성화
        messageText.gameObject.SetActive(false); // 메시지 텍스트 비활성화
        onCompleteCallback?.Invoke(); // 이후에 해야 하는 다른 액션이 있는 경우(null이 아님) 진행한다
        yield break;
    }

    IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 0.5f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            float alphaValue = Mathf.Lerp(0f, 1f, elapsedTime / fadedTime);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaValue);
            messageText.alpha = alphaValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        onCompleteCallback?.Invoke(); // 이후에 해야 하는 다른 액션이 있는 경우(null이 아님) 진행한다
        yield break;
    }

    IEnumerator WaitAndFadeIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (FirstsleepUI.activeSelf)
        {
            // FirstsleepUI를 비활성화
            FirstsleepUI.SetActive(false);
            Debug.Log("FirstsleepUI가 비활성화되었습니다.");
        }
        FadeIn();
    }
}