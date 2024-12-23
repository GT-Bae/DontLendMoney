using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Chating2 : MonoBehaviour
{
    public Text friendtx1;
    public Text friendtx2;
    public Text friendtx3;
    public Text mytx1;
    public Text mytx2;

    private string[] dialogues; // 대사 목록
    private Text[] textObjects; // 대사를 표시할 Text 컴포넌트 배열
    private int currentDialogueIndex = 0; // 현재 대화 인덱스
    private bool isTyping = false; // 타이핑 중인지 확인

    private void Start()
    {
        // 대화 설정
        dialogues = new string[] {
            "야 자고있냐?",            // 친구 대사 1
            "ㄴㄴ 게임 중 왜?",        // 내 대사 1
            "나 친구들이랑 술좀 먹게 10만원만 빌려줄 수 있음?", // 친구 대사 2
            "ㅇㅋㅇㅋ 돈 보내줄게"    // 내 대사 2
        };

        // Text 컴포넌트 배열 설정 (순서대로 friend, my, friend...)
        textObjects = new Text[] { friendtx1, mytx1, friendtx2, mytx2 };

        // 첫 번째 대사 초기화
        foreach (var text in textObjects)
        {
            text.text = ""; // 모든 텍스트 초기화
        }
    }

    private void Update()
    {
        // 스페이스 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping && currentDialogueIndex < dialogues.Length)
        {
            StartCoroutine(TypeText(textObjects[currentDialogueIndex], dialogues[currentDialogueIndex]));
            currentDialogueIndex++;
        }
    }

    IEnumerator TypeText(Text targetText, string dialogue)
    {
        isTyping = true;
        targetText.text = ""; // 기존 텍스트 초기화
        yield return new WaitForSeconds(0.5f); // 타이핑 전 대기
        for (int i = 0; i <= dialogue.Length; i++)
        {
            targetText.text = dialogue.Substring(0, i); // 한 글자씩 출력
            yield return new WaitForSeconds(0.1f); // 타이핑 속도
        }
        isTyping = false;
    }
}
