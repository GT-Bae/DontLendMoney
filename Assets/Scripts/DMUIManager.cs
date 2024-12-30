using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DMUIManager : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject optionsUI;
    public Transform content;
    public GameObject chatBoxPrefab;
    public float optionsHeight = 300f; // 선택지 UI의 높이

    private RectTransform scrollViewRect;
    private RectTransform contentRect;
    private ScrollRect scrollRect;
    private bool optionsActive = false;
    
    public TMP_Text chatNameText;

    void Start()
    {
        scrollViewRect = scrollView.GetComponent<RectTransform>();
        contentRect = content.GetComponent<RectTransform>();
        scrollRect = scrollView.GetComponent<ScrollRect>();
        optionsUI.SetActive(false); // 초기에는 선택지 UI 비활성화
    }

    void Update()
    {
        // 예시로 스페이스바를 눌러 선택지 UI를 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleOptionsUI();
        } else if (Input.GetKeyDown(KeyCode.L)) {
            AddMessage("응애");
        }
    }

    public void SetChatName(string name)
    {
        chatNameText.text = name;
    }

    public void ToggleOptionsUI()
    {   
        ScrollToBottom();
        optionsActive = !optionsActive;
        optionsUI.SetActive(optionsActive);

        if (optionsActive)
        {
            // 선택지 UI 활성화 시 ScrollView 크기 줄이기 및 위치 조정
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y - optionsHeight);
            scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y + optionsHeight / 2);

            // Content 위치 조정
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y + optionsHeight);
        }
        else
        {
            // 선택지 UI 비활성화 시 ScrollView 크기 원래대로 및 위치 조정
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y + optionsHeight);
            scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y - optionsHeight / 2);

            // Content 위치 조정
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y - optionsHeight);
        }

        // Canvas 업데이트 강제
        Canvas.ForceUpdateCanvases();
    }

    public void AddMessage(string message)
    {
        GameObject newChatBox = Instantiate(chatBoxPrefab, content);
        TMP_Text messageText = newChatBox.GetComponentInChildren<TMP_Text>();
        messageText.text = message;

        // Canvas 업데이트 강제
        Canvas.ForceUpdateCanvases();

        // ScrollView를 맨 아래로 스크롤
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        // ScrollRect를 맨 아래로 스크롤
        Canvas.ForceUpdateCanvases(); // 레이아웃 업데이트 후 스크롤
        scrollRect.verticalNormalizedPosition = 0f;
    }
}