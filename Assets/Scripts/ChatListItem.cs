using TMPro;
using UnityEngine;

public class ChatListItem : MonoBehaviour
{
    public GameObject dmUIPrefab;
    private GameObject currentDMUI;

    public void OnChatListItemClicked()
    {
        string chatName = transform.Find("Name").GetComponent<TMP_Text>().text;
        DMUIManager dmuiScript = currentDMUI.GetComponent<DMUIManager>();
        if (currentDMUI == null)
        {
            currentDMUI = Instantiate(dmUIPrefab);
        }
        else
        {
            currentDMUI.SetActive(true);
        }
        dmuiScript.SetChatName(chatName);
    }
}
