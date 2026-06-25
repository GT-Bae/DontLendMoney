/*
 * 残り体力に応じてチャット画面のクリックイベントを制御するクラス
 */

using TMPro;
using UnityEngine;

public class ChatListItem : MonoBehaviour
{
    public GameObject dmUIPrefab;
    public GameObject notice;
    public GameObject warningLowHealthUI;
    private GameObject currentDMUI;
    public bool hasChatted = false;

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

            DMUIManager dmuiScript = currentDMUI.GetComponent<DMUIManager>();
            dmuiScript.SetChatName(chatName);
            dmuiScript.hasChatted();
        } else {
            Instantiate(warningLowHealthUI);
            Debug.Log("体力が尽きています。");
        }        
    }
}