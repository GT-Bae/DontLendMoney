using UnityEngine;
using UnityEngine.UI;

public class ChatBoxTest : MonoBehaviour
{
    public GameObject chatBoxPrefab;
    public GameObject chatBoxPrefab2;
    public Transform content;

    void Start()
    {
        // 프리팹 생성 및 추가
        for (int i = 0; i < 5; i++)
        {
            GameObject newChatBox = Instantiate(chatBoxPrefab, content);
            GameObject newChatBox2 = Instantiate(chatBoxPrefab2, content);
        }

        // Canvas 업데이트 강제
        //Canvas.ForceUpdateCanvases();
    }
}