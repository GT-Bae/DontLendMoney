using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button nextButton;
    public string[] dialogues = {"이런 의문 가진적 없어?", "너의 진짜 친구는 몇 명일까?", "궁금하지 않아?"};
    public float textDisplayTime = 2f; // 텍스트가 표시되는 시간
    public OpenTyping openTyping; // TypingEffect 스크립트 참조

    private int currentDialogueIndex = 0;

    void Start()
    {
        StartCoroutine(DisplayDialogues());
    }

    IEnumerator DisplayDialogues()
    {
        while (currentDialogueIndex < dialogues.Length)
        {
            openTyping.SetText(dialogues[currentDialogueIndex]);
            currentDialogueIndex++;
            yield return new WaitForSeconds(textDisplayTime);
        }

        // 모든 대사가 끝나면 버튼 활성화
        nextButton.interactable = true;
    }
}