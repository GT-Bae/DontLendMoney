/* 
 * 코드 참고: https://hereishyun.tistory.com/100
 * FadeController 클래스는 패널과 텍스트의 불투명도를 조절하여 페이드 인/아웃 효과를 구현합니다.
 * 패널과 텍스트의 알파 값을 조절하여 페이드 인/아웃을 수행합니다.
 * FadeOutWithMessage 함수는 메시지를 화면에 표시하고 5초 후에 페이드 인을 실행합니다.
 * 다른 스크립트에서 콜백 함수를 등록할 수 있습니다.
 */

using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class FadeController : MonoBehaviour // Panel 불투명도 조절해 페이드인 or 페이드아웃
{
    public GameObject panel; // 불투명도를 조절할 Panel 오브젝트
    public TextMeshProUGUI messageText; // 화면에 표시할 텍스트 UI 요소
    private Action onCompleteCallback; // FadeIn 또는 FadeOut 다음에 진행할 함수

    void Start()
    {
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
        Debug.Log("FadeCanvasController_ Fade In 시작");
        StartCoroutine(CoFadeIn());
        Debug.Log("FadeCanvasController_ Fade In 끝");
    }

    public void FadeOut()
    {
        panel.SetActive(true); // Panel 활성화
        messageText.gameObject.SetActive(true); // 메시지 텍스트 활성화
        Debug.Log("FadeCanvasController_ Fade Out 시작");
        StartCoroutine(CoFadeOut());
        Debug.Log("FadeCanvasController_ Fade Out 끝");
    }

    public void FadeOutWithMessage(string message)
    {
        messageText.text = message; // 메시지 설정
        messageText.gameObject.SetActive(true); // 메시지 텍스트 활성화
        FadeOut();
        StartCoroutine(WaitAndFadeIn(5f)); // 5초 후에 FadeIn 호출
    }

    IEnumerator CoFadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 0.5f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            float alphaValue = Mathf.Lerp(1f, 0f, elapsedTime / fadedTime);
            panel.GetComponent<CanvasRenderer>().SetAlpha(alphaValue);
            messageText.alpha = alphaValue;

            elapsedTime += Time.deltaTime;
            Debug.Log("Fade In 중...");
            yield return null;
        }
        Debug.Log("Fade In 끝");
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
            Debug.Log("Fade Out 중...");
            yield return null;
        }

        Debug.Log("Fade Out 끝");
        onCompleteCallback?.Invoke(); // 이후에 해야 하는 다른 액션이 있는 경우(null이 아님) 진행한다
        yield break;
    }

    IEnumerator WaitAndFadeIn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        FadeIn();
    }

    public void RegisterCallback(Action callback) // 다른 스크립트에서 콜백 액션 등록하기 위해 사용
    {
        onCompleteCallback = callback;
    }
}