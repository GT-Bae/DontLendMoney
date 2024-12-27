using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BAKSIYUN_chating : MonoBehaviour
{
    public GameObject chatingPrefab;    // 대화 프리팹
    public Transform contentArea;       // ScrollView 콘텐츠 영역
    public Button button1;              // 버튼 1
    public Button button2;              // 버튼 2
    public Button exitButton;           // 나가는 버튼
    public Text moneyText;              // 현재 금액 표시 텍스트
    public Text healthText;             // 현재 체력 표시 텍스트
    public GameObject warningPrefab; // 경고 메시지 Prefab
    public Transform canvasTransform; // Prefab이 표시될 Canvas
    public string chatDataFileName = "UserData1_chating"; // JSON 파일 이름 (확장자 제외)

    private List<ChatData> allChatData = new List<ChatData>();
    private HashSet<int> displayedIndices = new HashSet<int>(); // 이미 출력된 대화 인덱스 저장
    private const string ProgressKey = "ChatEndIndex_BAKSIYUN"; // 진행 상태 저장 키
    private const string Button1Key = "Button1Clicked_BAKSIYUN";
    private const string Button2Key = "Button2Clicked_BAKSIYUN";
    private const string MoneyKey = "MyMoney"; // 플레이어 돈 저장 키
    private const string HealthKey = "CurrentHealth"; // 체력 저장 키
    private const string BorrowedDayKey_BAKSIYUN = "BorrowedDay_BAKSIYUN"; // 돈 빌린 날짜 키
    private const string CurrentDayKey = "CurrentDay"; // 현재 날짜 키

    private void Start()
    {
        LoadChatingData();

        // 현재 금액 및 체력 표시
        UpdateMoneyText();
        UpdateHealthText();

        // 버튼 초기 비활성화 및 화면에서 숨기기
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);

        // 버튼 상태 복원: 둘 중 하나라도 클릭된 상태면 두 버튼 모두 비활성화
        if (PlayerPrefs.GetInt(Button1Key, 0) == 1 || PlayerPrefs.GetInt(Button2Key, 0) == 1)
        {
            button1.interactable = false;
            button2.interactable = false;
        }

        // 마지막 진행 상태 확인 및 복원
        int savedEndIndex = PlayerPrefs.GetInt(ProgressKey, 11); // 기본값: 9~11 대화

        // 버튼 상태에 따라 대화 복원
        if (PlayerPrefs.GetInt(Button2Key, 0) == 1) // 버튼 2 선택 시
        {
            StartCoroutine(DisplayChatWithTypingEffect(9, 11, skipTyping: true));
            StartCoroutine(DisplayChatWithTypingEffect(14, 15, skipTyping: true));
        }
        else
        {
            StartCoroutine(DisplayChatWithTypingEffect(9, savedEndIndex, skipTyping: savedEndIndex > 11));
        }

        // 빌린 돈이 있는지 확인하고 갚는 처리 실행
        CheckRepayment();

        // 나가는 버튼 클릭 이벤트 등록
        exitButton.onClick.AddListener(ReturnToMainScene);

        // 버튼 클릭 이벤트 등록
        button1.onClick.AddListener(() => OnButtonClicked(button1, 12, 13, true));
        button2.onClick.AddListener(() => OnButtonClicked(button2, 14, 15, false));
    }

    void LoadChatingData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(chatDataFileName);
        if (textAsset != null)
        {
            string jsonData = textAsset.text;

            if (!jsonData.TrimStart().StartsWith("{"))
            {
                jsonData = "{\"chating\":" + jsonData + "}";
            }

            ChatDataList chatList = JsonUtility.FromJson<ChatDataList>(jsonData);

            allChatData = chatList.chating;
        }
        else
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다. 경로: Resources/{chatDataFileName}.json");
        }
    }

    void UpdateMoneyText()
    {
        float currentMoney = PlayerPrefs.GetFloat(MoneyKey, 10f); // 기본값 10
        moneyText.text = $"{currentMoney}만원";
    }

    void UpdateHealthText()
    {
        int currentHealth = PlayerPrefs.GetInt(HealthKey, 3); // 기본 체력 3
        healthText.text = $"체력: {currentHealth}";
    }

    IEnumerator DisplayChatWithTypingEffect(int startIdx, int endIdx, bool skipTyping = false)
    {
        for (int i = startIdx; i <= endIdx && i < allChatData.Count; i++)
        {
            if (displayedIndices.Contains(i))
                continue;

            displayedIndices.Add(i);

            if (skipTyping)
            {
                CreateChating(allChatData[i]);
            }
            else
            {
                CreateChatingWithTyping(allChatData[i]);
                yield return new WaitForSeconds(1f);
            }
        }

        if (endIdx >= 11)
        {
            button1.gameObject.SetActive(true);
            button2.gameObject.SetActive(true);
        }
    }

    void OnButtonClicked(Button clickedButton, int startIdx, int endIdx, bool isButton1)
    {
        button1.interactable = false;
        button2.interactable = false;

        if (isButton1)
        {
            float currentMoney = PlayerPrefs.GetFloat(MoneyKey); // 현재 금액 가져오기
            int currentHealth = PlayerPrefs.GetInt(HealthKey);// 현재 체력 가져오기

            if (currentHealth <= 0) // 체력이 부족한 경우
            {
                ShowWarning(); // 체력 부족 경고 메시지
                button1.interactable = true;
                button2.interactable = true;
                return;
            }
            if (currentMoney >= 10) // 금액이 충분할 경우에만 대화 진행
            {
                DeductMoney(10); // 돈 차감
                DeductHealth(1); // 체력 차감
                SaveBorrowedDay(); // 빌린 날짜 저장
                PlayerPrefs.SetInt(Button1Key, 1); // 버튼 1 누른 상태 저장
                PlayerPrefs.SetInt(ProgressKey, endIdx); // 진행 상태 저장
                PlayerPrefs.Save();
                StartCoroutine(DisplayChatWithTypingEffect(startIdx, endIdx)); // 대화 진행
            }
            else
            {
                ShowWarning(); // 경고 메시지 출력
                               // 버튼 다시 활성화
                button1.interactable = true;
                button2.interactable = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt(Button2Key, 1); // 버튼 2 누른 상태 저장
            PlayerPrefs.SetInt(ProgressKey, endIdx); // 진행 상태 저장
            PlayerPrefs.Save();
            StartCoroutine(DisplayChatWithTypingEffect(startIdx, endIdx)); // 거절 대화 진행
        }
    }


    void DeductMoney(float amount)
    {
        float currentMoney = PlayerPrefs.GetFloat(MoneyKey);
        currentMoney -= amount;

        if (currentMoney < 0)
        {
            currentMoney = 0;
            Debug.LogWarning("돈이 부족합니다.");
        }

        PlayerPrefs.SetFloat(MoneyKey, currentMoney);
        PlayerPrefs.Save();

        UpdateMoneyText();
    }

    void DeductHealth(int amount)
    {
        int currentHealth = PlayerPrefs.GetInt(HealthKey);
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            Debug.LogWarning("체력이 부족합니다.");
        }

        PlayerPrefs.SetInt(HealthKey, currentHealth);
        PlayerPrefs.Save();

        UpdateHealthText();
    }

    void SaveBorrowedDay()
    {
        int currentDay = PlayerPrefs.GetInt(CurrentDayKey);
        PlayerPrefs.SetInt(BorrowedDayKey_BAKSIYUN, currentDay);
        PlayerPrefs.Save();
    }

    void CheckRepayment()
    {
        int borrowedDay = PlayerPrefs.GetInt(BorrowedDayKey_BAKSIYUN, -1);
        int currentDay = PlayerPrefs.GetInt(CurrentDayKey);

        if (borrowedDay > 0 && currentDay == borrowedDay - 1)
        {
            HandleDebtRepayment();
        }
    }

    void HandleDebtRepayment()
    {
        float randomValue = Random.value;
        float currentMoney = PlayerPrefs.GetFloat(MoneyKey );

        if (randomValue <= 0.7f)
        {
            currentMoney += 10;
            Debug.Log("원금갚음");
            CreateChating(allChatData[16]);
        }
        else if (randomValue <= 0.85f)
        {
            currentMoney += 15;
            Debug.Log("원금+이자까지 갚음");
            CreateChating(allChatData[17]);
        }
        else
        {
            Debug.Log("잠수탔습니다.");
        }

        PlayerPrefs.SetFloat(MoneyKey, currentMoney);
        PlayerPrefs.DeleteKey(BorrowedDayKey_BAKSIYUN);
        PlayerPrefs.Save();

        UpdateMoneyText();
    }

    void ReturnToMainScene()
    {
        SceneManager.LoadScene("masaage");
    }

    void CreateChating(ChatData data)
    {
        GameObject chatObject = Instantiate(chatingPrefab, contentArea);

        Text[] texts = chatObject.GetComponentsInChildren<Text>();
        if (texts.Length >= 2)
        {
            texts[0].text = data.speaker;
            texts[1].text = data.message;
        }
    }

    void CreateChatingWithTyping(ChatData data)
    {
        GameObject chatObject = Instantiate(chatingPrefab, contentArea);

        Text[] texts = chatObject.GetComponentsInChildren<Text>();
        if (texts.Length >= 2)
        {
            texts[0].text = data.speaker;
            StartCoroutine(TypeMessage(texts[1], data.message));
        }
    }

    IEnumerator TypeMessage(Text messageText, string message)
    {
        messageText.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            messageText.text += message[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
    void ShowWarning()
    {
        GameObject warningInstance = Instantiate(warningPrefab, canvasTransform);
        // 3초 후 경고 메시지 비활성화
        Destroy(warningInstance, 3f);
    }
}
