/*
 * オープニングの台詞を表示し、終了後に進行ボタンを有効化するクラス
 */

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button nextButton;
    private List<string> dialogues = new List<string>();
    public float textDisplayTime = 3f;
    public TypingEffect typing;

    private int currentDialogueIndex = 0;

    void Start()
    {
        string playerName = PlayerPrefs.GetString("Name", "ブランク");
        dialogues.Add($"ねぇ。{playerName}、あんたの財産はすべて私の手の中にあるよ。");
        dialogues.Add("28日間、お金を貸してほしいっていう連絡が来るはずだから。");
        dialogues.Add("せいぜい、頑張ることね。");

        StartCoroutine(DisplayDialogues());
    }

    IEnumerator DisplayDialogues()
    {
        while (currentDialogueIndex < dialogues.Count)
        {
            typing.SetText(dialogues[currentDialogueIndex]);
            currentDialogueIndex++;
            yield return new WaitForSeconds(textDisplayTime);
        }

        nextButton.interactable = true;
    }
}