using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class ChatListItem : MonoBehaviour
{
    public GameObject dmUIPrefab;
    public GameObject notice;
    public GameObject warningLowHealthUI;
    private GameObject currentDMUI;
    public bool hasChatted = false; // 처음에는 false로 설정

    public void OnChatListItemClicked()
    {   
        int health = PlayerPrefs.GetInt("CurrentHealth",0);
        if (health > 0) {
            string chatName = transform.Find("Name").GetComponent<TMP_Text>().text;
            if (currentDMUI == null)
            {
                notice.SetActive(false);
                hasChatted = true;
                currentDMUI = Instantiate(dmUIPrefab);
            }
            else
            {
                currentDMUI.SetActive(true);
            }

            // DMUI 스크립트에 접근하여 SetChatName 호출
            DMUIManager dmuiScript = currentDMUI.GetComponent<DMUIManager>();
            dmuiScript.SetChatName(chatName);
            dmuiScript.hasChatted();
        } else {
            Instantiate(warningLowHealthUI);
            Debug.Log("체력이 부족합니다");
        }        
    }
}