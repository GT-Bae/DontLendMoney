using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Collections;

public class DMUIManager : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject optionsUIPrefab;
    public Transform content;
    public GameObject otherChatPrefab;
    public GameObject myChatPrefab;
    public float optionsHeight = 300f; // 선택지 UI의 높이
    public GameObject warningLowMoney;
    private GameObject optionsUI = null;
    private RectTransform scrollViewRect;
    private RectTransform contentRect;
    private ScrollRect scrollRect;
    private bool optionsActive = false;
    private GameObject gameEventObject;
    public TMP_Text chatNameText;
    public List<Sprite> profiles;
    private bool hasChat = false;
    private bool choiceMade = false;
    private List<string> FriendBorrowMents = new List<string> {"급한 사정이 있어서 그런데","면목이 없는데","미안한데","오랜만에 연락해서 이런말 해서 미안한데","이런말 하기 부끄러운데","이러면 안 되는거 아는데","정말 미안하다","잘 지내니?","요즘 힘들어서 그런데","어려운 부탁해서 미안한데","상황이 안 좋아서 그런데","이렇게 부탁하게 될 줄은 몰랐는데","도와주면 좋을 것 같다. 미안하다.","불편한 부탁일텐데","어쩔수 없는 상황이라 그런데","도와주면 정말 잊지 않을게","혹시 조금만 도와줄 수 있겠니?","부탁좀 해도 될까?","이런 부탁해서 미안한데","미안한 부탁인데","연락없다가 부탁해서 미안한데","면목없는 부탁인데","지푸라기 잡는 심정으로 부탁할게","방법이 없어서 그런데","바쁠텐데 정말 미안한데","바쁘니? 부탁 하나만 할게","잘 지내지? 상황이 어쩔수 없어서 그런데","한번만 빌려주면 안 될까?","돈이 부족해서 그런데","어쩔 수 없어서 부탁하는건데","여자친구 선물 줘야해서 그런데","생활비 때문에 그런데","병원비 때문에 그런데","간병비 때문에 그런데","술먹어야 해서 그런데","카드값 내야해서 그런데","주식을 바로 못 빼서 그런데","친구랑 여행가야 해서 그런데","돈이 조금 부족해서 그런데","폰값 빨리 내야해서 그런데","식비가 족해서 그런데","급전이 필요해서 그런데","대출이 안 되어서 그런데","투자 망해서 돈이 없어서 그런데","대출 갚아야 해서 그런데","갑자기 모임이 생겨서 그런데","갑자기 큰 일이 생겨 런데","예상치 못하게 짤려서 그런데","알바를 해도 돈이 부족해서 그런데","갑자기 집안에 조사가 생겨서 그런데","중요한 약속이 있어서 그런데"};
    private List<int> borrowAmount = new List<int> {10,15,20,25,30,35,40,45,50};
    private List<string> FriendAgreeMents = new List<string> {"역시 내 친구다 고마워","진짜 고마워","고마워 ㅠㅠ","친구야 고마워","사랑한다 친구야!!","고마워 친구야","친구야 잊지 않을게","꼭 갚을게 친구야","친구야… 힘든 결정이었을 텐데 고워","갑작스러운 부탁이었는데 고마워 친구야!!","친구야 이 은혜 잊지 않을게","믿어줘서 고마워 친구야","친구야 확실히 갚을게!!","바쁠 텐데 고마워","덕분에 힘이 난다 친구야","친구야 덕분에 살았다","도와줘서 정말 고마워 친구야","친구야 덕분에 해결했어! 꼭 갚을게","고마워 꼭 갚을게","친구야 네가 있어서 다행이다","꼭 갚을게","기억하고 있을게","확실히 갚을게","어려운 결정이었을 텐데 고마워 친구야","큰일 날뻔했는데 고마워"};
    private List<string> FriendDenyMents = new List<string> {"니가 그러고도 친구냐?","실망이다 친구야","친구가 힘들어하는데 도와주지도 않네","친구야 이럴거야?","우리의 우정이 겨우 이 정도였어?","진심으로 실망이다","나 무시하는거야?","내가 힘들다는데!!!","친구야 내가 뭐 잘못한거 있니?","이건 좀 아니지 않니?","이러면 부탁한 나는 어떤 기분이 들겠니?","그게 다야?","너, 나랑 친구 맞아?","친구가 도와달라는데 이래도 돼?","친구를 도와주기 싫어?","그래","어쩔 수 없지 뭐","네 상황도 있을거니까 이해해","알겠어","미안해","그래 볼일 봐","바쁠텐데 미안해","그냥 무시해 내가 생각해도 이건 아니다","미안해 친구한테 빌리는건 확실히 아닌 것 같다","방해해서 미안해","내가 너무 성급했다 미안","미안해 신경쓰이게 해서","말도 안 되는 부탁이긴 했다 미안해","그래 나중에 밥이나 한번 먹자","그래 잘 지내"};

    void Awake()
    {
        scrollViewRect = scrollView.GetComponent<RectTransform>();
        contentRect = content.GetComponent<RectTransform>();
        scrollRect = scrollView.GetComponent<ScrollRect>();
    }

    public void SetChatName(string name)
    {
        chatNameText.text = name;

        if (name != "악마" && name != "김민준" && name != "고훈이") {
            AddOtherChat();
        } else if (name == "악마") {
            DemonChat();
        } else if (name == "김민준") {
            BFFChat();
        } else if (name == "고훈이") {
            VillainChat();
        }
    }

    public void ToggleOptionsUI()
{
    ScrollToBottom();
    optionsActive = !optionsActive;
    
    if (optionsActive)
    {
        optionsUI = Instantiate(optionsUIPrefab);
        // ScrollView 크기 줄이기 및 위치 조정
        scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y - optionsHeight);
        scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y + optionsHeight / 2);

        // Content 위치 조정
        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y + optionsHeight);
    }
    else
    {
        // ScrollView 크기 원래대로 및 위치 조정
        scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y + optionsHeight);
        scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y - optionsHeight / 2);

        // Content 위치 조정
        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y - optionsHeight);

        Destroy(optionsUI);
    }

    // Canvas 업데이트 강제
    Canvas.ForceUpdateCanvases();
}

    public void AddMessage(string message)
    {
        GameObject newChatBox = Instantiate(otherChatPrefab, content);
        TMP_Text[] textComponents = newChatBox.GetComponentsInChildren<TMP_Text>();
        Image[] profile = newChatBox.GetComponentsInChildren<Image>();

        TMP_Text nameText = null;
        TMP_Text messageText = null;

        foreach (TMP_Text textComponent in textComponents)
        {
            if (textComponent.name == "Name")
            {
                nameText = textComponent;
            }
            else if (textComponent.name == "Content")
            {
                messageText = textComponent;
            }
        }

        nameText.text = chatNameText.text;
        messageText.text = message;

        foreach (Image img in profile)
        {
            if (img.name == "Profile")
            {
                if (nameText.text == "악마") {
                    img.sprite = profiles[0];
                } else if (nameText.text == "김민준") {
                    img.sprite = profiles[1];
                } else if (nameText.text == "고훈이") {
                    img.sprite = profiles[2];
                }
            }
        }

        // Canvas 업데이트 강제
        Canvas.ForceUpdateCanvases();

        // ScrollView를 맨 아래로 스크롤
        ScrollToBottom();
    }

    public void AddMyMessage(string message)
    {
        GameObject newChatBox = Instantiate(myChatPrefab, content);
        TMP_Text messageText = newChatBox.GetComponentInChildren<TMP_Text>();
        messageText.text = message;

        // Canvas 업데이트 강제
        Canvas.ForceUpdateCanvases();

        // ScrollView를 맨 아래로 스크롤
        ScrollToBottom();
    }

    private IEnumerator SmoothScrollToBottom()
    {
        float duration = 0.5f; // 스크롤이 완료되는 데 걸리는 시간 (초)
        float elapsedTime = 0f;
        float startValue = scrollRect.verticalNormalizedPosition;
        float endValue = 0f; // 맨 아래로 스크롤

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = endValue; // 최종 위치 설정
    }

private void ScrollToBottom()
{
    if (scrollRect != null)
    {
        Canvas.ForceUpdateCanvases(); // 레이아웃 업데이트 후 스크롤
        StartCoroutine(SmoothScrollToBottom());
    }
    else
    {
        Debug.LogError("ScrollRect is null!");
    }
}

    private void AddOtherChat()
    {   
        if (hasChat == false) {
            int maxFriends = PlayerPrefs.GetInt("MaxFriends",0);
            PlayerPrefs.SetInt("MaxFriends",maxFriends+1);
            int numFriends = PlayerPrefs.GetInt("CurrentFriends", 0);
            PlayerPrefs.SetInt("CurrentFriends",numFriends+1);
            // 랜덤 멘트와 금액 설정
            string randomMent = FriendBorrowMents[Random.Range(0, FriendBorrowMents.Count)];
            int randomAmount = borrowAmount[Random.Range(0, borrowAmount.Count)];
            int randomDate = Random.Range(1,4);

            string playerName = PlayerPrefs.GetString("Name", "민수");

            AddMessage($"{playerName}야 {randomMent}\n{randomAmount}만원 빌려줄 수 있어?");
            AddMessage($"{randomDate}일 뒤에 갚을게");
            
            // 선택지 UI 활성화
            ToggleOptionsUI();

            // 버튼 텍스트 설정
            Button button1 = optionsUI.transform.Find("Button1").GetComponent<Button>();
            Button button2 = optionsUI.transform.Find("Button2").GetComponent<Button>();
            button1.GetComponentInChildren<TMP_Text>().text = "빌려준다";
            button2.GetComponentInChildren<TMP_Text>().text = "안 빌려준다";

            // 버튼 클릭 이벤트 설정
            button1.onClick.AddListener(() => OnAgree(randomAmount,randomDate));
            button2.onClick.AddListener(() => OnDeny());
        } else {
            Debug.Log("이미 본 대화입니다.");
        }
    }

    private void OnAgree(int randomAmount, int randomDate)
    {
        int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
        if (currentMoney < randomAmount) {
            Instantiate(warningLowMoney);
        } else {
            GameObject gameEventObject = GameObject.Find("GameManager");
            GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
            PlayerPrefs.SetInt("MyMoney",currentMoney-randomAmount);
            AddMyMessage("그래");

            // 랜덤 동의 멘트 추가
            string randomAgreeMent = FriendAgreeMents[Random.Range(0, FriendAgreeMents.Count)];
            AddMessage(randomAgreeMent);

            int health = PlayerPrefs.GetInt("CurrentHealth",0);
            PlayerPrefs.SetInt("CurrentHealth",health-1);

            gameEvent.AddToList(chatNameText.text, randomAmount, randomDate);
            gameEvent.UpdateAllText();

            // 선택지 UI 비활성화
            ToggleOptionsUI();
        }
    }

    private void OnDeny()
    {
        AddMyMessage("싫어");
        // 랜덤 거절 멘트 추가
        string randomDenyMent = FriendDenyMents[Random.Range(0, FriendDenyMents.Count)];
        AddMessage(randomDenyMent);

        // CurrentFriends 값 -1
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0);
        PlayerPrefs.SetInt("CurrentFriends", currentFriends - 1);
        
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        gameEvent.UpdateAllText();
        
        // 선택지 UI 비활성화
        ToggleOptionsUI();
    }

    public void hasChatted() {
        hasChat = true;
    }

    public IEnumerator WaitForThreeSeconds()
    {
        yield return new WaitForSeconds(3f);
    }
    
    public IEnumerator WaitForChoice()
    {
        choiceMade = false;
        yield return new WaitUntil(() => choiceMade);
    }

    private void OnChoiceMade()
    {
        choiceMade = true;
    }

    public void DemonChat() {
        StartCoroutine(DemonChatCoroutine());
    }

    public void BFFChat() {
        StartCoroutine(BFFChatCoroutine());
    }

    public void VillainChat() {
        StartCoroutine(VillainCahatCoroutine());
    }

    public IEnumerator DemonChatCoroutine() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay",0);
        string name = PlayerPrefs.GetString("Name","민수");
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        if (currentDay == 1) {
            AddMessage("나 알지?");
            ToggleOptionsUIStory("뭔데 내 친구에 대해 궁금해 하는거야?","누구신데 처음부터 반말이야?");
            yield return WaitForChoice();          
            AddMessage("궁금하지 않아? 내기 하나 하자");
            ToggleOptionsUIStory("딱히 궁금하진 않은데", "그냥 살면 안돼?");
            yield return WaitForChoice();
            AddMessage("28일 동안 네 친구들한테서 돈빌려 달라는 메시지가 올거야.");
            yield return new WaitForSeconds(2f);
            AddMessage("그러고 마지막 날까지 최대한 많은 돈을 돌려받아봐.");
            ToggleOptionsUIStory("안 하고 싶은데", "내 손해 아니야?");
            yield return WaitForChoice();
            AddMessage("돌려받은 돈의 3배를 줄게. 어때?");
            ToggleOptionsUIStory("괜찮은데?", "나쁜데?");
            yield return WaitForChoice();
            AddMessage($"됐고! 계약서나 작성하자. 이름은 {name}이고 기간은 28일…");
            yield return new WaitForSeconds(2f);
            AddMessage("…좋아… 그럼 28일 동안 잘 부탁해!");
            ToggleOptionsUIStory("싫어", "계약 파기할래");
            yield return WaitForChoice();
            AddMessage("(오프라인 상태입니다. - 연락금지)");
        } else if (currentDay == 5) {
            AddMessage("고훈이가 돈 빌려달라하면 꼭 빌려줘라");
            ToggleOptionsUIStory("왜?","싫은데?");
            yield return WaitForChoice();
            AddMessage("왠지 저녀석 내 마음에 들거든");
            yield return new WaitForSeconds(2f);
            AddMessage("안 빌려주면 그냥 네 돈 갖고 돌아갈거야");
            yield return new WaitForSeconds(2f);
            AddMessage("계약서 2조 3항에 적혀있어");
            ToggleOptionsUIStory("치밀하네","계약서 6조 2항에 나한테 맞아야 한다는건 없어?");
            yield return WaitForChoice();
            AddMessage("(오프라인 상태입니다. - 연락금지)");
        } else if (currentDay == 8) {
            AddMessage("그냥 두니까 재미 없는데");
            ToggleOptionsUIStory("뭔 소리야?", "이젠 뭐 또 할 건데?");
            yield return WaitForChoice();
            AddMessage("앞으로 금요일마다 나한테 30만 원씩 헌납해.");
            ToggleOptionsUIStory("생 양아치 아니야?", "진짜 이건 아니다");
            yield return WaitForChoice();
            AddMessage("나는 월급도 안 받고 너 괴롭혀주는데 억울해서 못 살겠어서 그런다 왜");
            ToggleOptionsUIStory("내 월급이 줄잖아", "그럼 나는?");
            yield return WaitForChoice();
            AddMessage("(오프라인 상태입니다. - 연락금지)");
        } else if (currentDay == 15) {
            int currentHealth = PlayerPrefs.GetInt("CurrentHealth", 0);
            int maxHealth = PlayerPrefs.GetInt("MaxHealth", 0);
            AddMessage("이미지를 보냈어요.");
            AddMessage("동영상을 보냈어요.");
            yield return new WaitForSeconds(2f);
            AddMessage("돈을 보냈어요.");
            ToggleOptionsUIStory("뭐하냐?", "빠큐를 보냈어요.");
            yield return WaitForChoice();
            AddMessage("내가 요즘 힘이 없는 것 같아서");
            yield return new WaitForSeconds(2f);
            AddMessage("당분간 네 체력 1개 압수야.");
            ToggleOptionsUIStory("아픈 건 네 잘못인데 왜 나한테 이래?", "나도 네 뿔 1개 압수할래");
            yield return WaitForChoice();
            AddMessage("시끄러, 몸살 때문에 잘 거야.");
            ToggleOptionsUIStory("이러고 그냥 가는 거야?", "몸조리는 개뿔 얼어죽어라");
            yield return WaitForChoice();
            AddMessage("(오프라인 상태입니다. - 연락금지)");
            PlayerPrefs.SetInt("CurrentHealth",currentHealth-1);
            PlayerPrefs.SetInt("MaxHealth",maxHealth-1);
            gameEvent.UpdateAllText();
        } else if (currentDay == 22) {
            AddMessage("너 뭐 대출한 거 있냐?");
            ToggleOptionsUIStory("있으면 어쩔 건데?", "몰라도 되는 거 아니야?");
            yield return WaitForChoice();
            int hasLoan = PlayerPrefs.GetInt("hasLoan",0);
            if (hasLoan == 0)
                AddMessage("별거 아니야, 할 일이나 해.");
            else
                AddMessage("있네? 일일 이자를 5%로 올렸으니 잘 갚도록 해.");
            ToggleOptionsUIStory("뭐야?", "야");
            yield return WaitForChoice();
            AddMessage("(오프라인 상태입니다. - 연락금지)");
        } else if (currentDay == 28) {
            AddMessage("벌써 마지막 날이네.");
            ToggleOptionsUIStory("어쩌라고", "잘가라");
            yield return WaitForChoice();
            AddMessage("드디어 이 썩어빠진 집구석을 떠나는구나.");
            ToggleOptionsUIStory("뭐야", "내 집에 있었어?");
            yield return WaitForChoice();
            AddMessage("혼자 살면 적적하니까 방해하려고 온 거야.");
            ToggleOptionsUIStory("청소나 하지", "빨래나 하지");
            yield return WaitForChoice();
            AddMessage("시끄러! 내일 보자.");
            ToggleOptionsUIStory("뭐지?", "야 일어나봐");
            yield return WaitForChoice();
            AddMessage("(오프라인 상태입니다. - 연락금지)");
        }
    }

    public IEnumerator BFFChatCoroutine() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay",0);
        string name = PlayerPrefs.GetString("Name","민수");
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        if (currentDay == 1) {
            AddMessage("야, 잘 지냄?");
            ToggleOptionsUIStory("ㅇㅇ", "너보다");
            yield return WaitForChoice();
            AddMessage("요놈.. 살아는 있네 ㅋㅋ");
            ToggleOptionsUIStory("반사", "시체가 말을 하네");
            yield return WaitForChoice();
            AddMessage("죽을래?!");
        } else if (currentDay == 3) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage($"{name}");
            ToggleOptionsUIStory("왜?", "무슨 일이야?");
            yield return WaitForChoice();
            AddMessage("20만 원만 빌릴게. 3일 뒤에 갚기 가능");
            ToggleOptionsUIStory("ㅇㅋ", "안 갚으면 디진다");
            yield return WaitForChoice();
            AddMessage("안갚으면 니 앞에서 간장샤워할게");
            PlayerPrefs.SetInt("MyMoney",currentMoney-20);
            gameEvent.UpdateAllText();
        } else if (currentDay == 6) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage("잘 썼당, 땡큐땡큐!");
            AddMessage("[25만 원 송금]");
            ToggleOptionsUIStory("나이스", "다행이네");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney+25);
            int recovery = PlayerPrefs.GetInt("Recovery",0);
            PlayerPrefs.SetInt("Recovery",recovery+25);
            AddMessage("수고!");
            gameEvent.UpdateAllText();
        } else if (currentDay == 15) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage("생축 ㅋㅋ");
            AddMessage("[50만 원 송금]");
            ToggleOptionsUIStory("뭐임?", "뭐 잘못 먹음?");
            yield return WaitForChoice();
            AddMessage("너도 내 생일 때 보냈잖아.");
            yield return new WaitForSeconds(2f);
            AddMessage("기대할게~~");
            ToggleOptionsUIStory("조졌네", "감사");
            yield return WaitForChoice();
            AddMessage("그래 들어가고");
            PlayerPrefs.SetInt("MyMoney",currentMoney+50);
            gameEvent.UpdateAllText();
        }
    }

    public IEnumerator VillainCahatCoroutine() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay",0);
        string name = PlayerPrefs.GetString("Name","민수");
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        if (currentDay == 2) {
            AddMessage("야 잘 지내냐?");
            ToggleOptionsUIStory("그래, 잘 지낸다.", "너는?");
            yield return WaitForChoice();
            AddMessage("다행이네.");
            ToggleOptionsUIStory("갑자기 왜?", "?");
            yield return WaitForChoice();
            AddMessage("안부 인사나 하는 거지 뭐.");
        } else if (currentDay == 4) {
            AddMessage("별일 없지?");
            ToggleOptionsUIStory("유튜브나 보고 있지 뭐.", "열심히 보고.");
            yield return WaitForChoice();
            AddMessage("나는 인스타 열심히 보는 중.");
            ToggleOptionsUIStory("열심히 보고.", "그래.");
            yield return WaitForChoice();
        } else if (currentDay == 7) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage($"{name}, 갑자기 부탁해서 미안한데 내일 돈 쓸 일이 생겼다. 진짜 이틀 뒤까지 갚을 수 있는데 계좌이체 좀 해 줄 수 있니?");
            ToggleOptionsUIStory("무슨 일인데?", "갑자기?");
            yield return WaitForChoice();
            AddMessage("내일 어머니 생신인데 이번까지 한 번도 선물 안 해드렸었거든. 돈 옮기다가 한도 때문에 막혀버렸어.");
            yield return new WaitForSeconds(2f);
            AddMessage("진짜 돈은 이틀 뒤에 꼭 줄게.");
            ToggleOptionsUIStory("몇 원 필요한데?", "그래.");
            yield return WaitForChoice();
            AddMessage("50만 원이면 돼.");
            PlayerPrefs.SetInt("MyMoney",currentMoney-50);
            yield return new WaitForSeconds(2f);
            AddMessage("아, 진짜 고맙다 ㅠ 꼭 이틀 뒤에 보낼게.");
            gameEvent.UpdateAllText();
        } else if (currentDay == 10) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            ToggleOptionsUIStory("어제까지 갚아야 했던 거 아니야?","안녕하세요?");
            yield return WaitForChoice();
            AddMessage($"{name}, 내가 수도권에 안 사는데 일 있어서 올라왔다가 돈이 없어서 집에 못 가고 있다.");
            ToggleOptionsUIStory("뭐라고?", "요즘 스마트폰으로 은행 앱 잘되잖아.");
            yield return WaitForChoice();
            AddMessage("아, 진짜 미안. 내가 최근에 적금 깼는데 그거 들어오면 줄려고 했어.");
            yield return new WaitForSeconds(2f);
            AddMessage("전화해보니까 이틀 뒤에 들어온대.");
            yield return new WaitForSeconds(2f);
            AddMessage("일단 자야 할 것 같은데 숙박비 줄 수 있니?");
            ToggleOptionsUIStory("일 때문에 수도권 갔는데 돈이 없다고?", "정신이 있는 거야?");
            yield return WaitForChoice();
            AddMessage("진짜 미안. 정말 지금 사정이 있어서 그렇다.");
            yield return new WaitForSeconds(2f);
            AddMessage("숙박비랑 교통비랑 식비 좀 보태줘라.");
            ToggleOptionsUIStory("장난하냐?", "몇 원 필요한데?");
            yield return WaitForChoice();
            AddMessage("숙박비 10 + 교통비 10 + 식비 1 = 21만 원이면 된다.");
            ToggleOptionsUIStory("정신 차리고 다녀라.", "돌겠네.");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney-21);
            AddMessage("믿어줘서 고맙다. 진짜 이틀 뒤에 보낼게.");
            gameEvent.UpdateAllText();
        } else if (currentDay == 13) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            ToggleOptionsUIStory("양치기 소년도 거짓말 두 번 했어.","음메헤헤헤");
            yield return WaitForChoice();
            AddMessage("내가 사실 저번 달에 소액결제 한 게 있어서 그거 갚아야 하는데.\n100만원만 빌릴 수 있을까?");
            ToggleOptionsUIStory("100만 원이 개 이름이냐?", "장난해?");
            yield return WaitForChoice();
            AddMessage("이번이 마지노선이라 안 갚으면 큰일 난다. 제발.");
            ToggleOptionsUIStory("뭐 했길래 큰일 나는데?", "그래서?");
            yield return WaitForChoice();
            AddMessage("나 사실대로 말할게.");
            yield return new WaitForSeconds(4f);
            AddMessage("사실 나 불법도박했어.");
            yield return new WaitForSeconds(2f);
            AddMessage("이제 정신 차리고 평범한 삶 살려고 한다.");
            ToggleOptionsUIStory("그래서?", "어쩌라고?");
            yield return WaitForChoice();
            AddMessage("진짜 100만 원만 빌려줘. 이틀 뒤 월급날에 줄게.");
            ToggleOptionsUIStory("도박하지 마라.", "미치겠네.");
            yield return WaitForChoice();
            AddMessage("아, 진짜 절대 안 한다. 고훈이 제발 정신 차리자.");
            yield return new WaitForSeconds(2f);
            PlayerPrefs.SetInt("MyMoney",currentMoney-100);
            AddMessage("믿고 빌려줘서 고마워ㅠ 월급 받자마자 줄게");
            gameEvent.UpdateAllText();
        } else if (currentDay == 16) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            ToggleOptionsUIStory("월급은 어디갔니?","이게 뭔 시츄에이션이야?");
            yield return WaitForChoice();
            AddMessage("아, 진짜 미안하다. 친구가 신고한다고 해서 그 친구부터 줬다.");
            ToggleOptionsUIStory("장난하니?", "뭐 하자는 거야?");
            yield return WaitForChoice();
            AddMessage("진짜로 나는 진심이다.");
            yield return new WaitForSeconds(2f);
            AddMessage("야간 알바도 뛰면서 돈 메꾸고 있다.");
            yield return new WaitForSeconds(2f);
            AddMessage("믿어줘라. 돈은 꼭 갚을게.");
            ToggleOptionsUIStory("그래, 꼭 갚아야지.", "약속은 지켜야 하는 거 알지?");
            yield return WaitForChoice();
            AddMessage("그래서 그런데 월세 90만 원 밀렸는데 빌려줄 수 있니?");
            yield return new WaitForSeconds(2f);
            AddMessage("진짜 급해서 그런다.");
            ToggleOptionsUIStory("돈 갚을 생각은 있냐?", "진짜 장난하냐?");
            yield return WaitForChoice();
            AddMessage("진짜 절실하다. 나 알바도 뛰고 있어서 많이 대화 못한다.");
            yield return new WaitForSeconds(2f);
            AddMessage("90만 원만 빌려줘라. 진짜 이틀 뒤에 꼭 갚을게.");
            ToggleOptionsUIStory("꼭 갚아라.", "내가 왜 이럴까");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney-90);
            AddMessage("정말로 갚을 수 있다. 고맙다.");
            gameEvent.UpdateAllText();
        } else if (currentDay == 19) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            int bigMoney = 0;
            ToggleOptionsUIStory("야.","뭐하냐?");
            yield return WaitForChoice();
            AddMessage("내가 도박했었잖아. 그걸로 신고 먹어서 벌금부터 냈다.");
            ToggleOptionsUIStory("그래서?", "나한테 그런 말은 왜 하는 걸까?");
            yield return WaitForChoice();
            AddMessage("벌금 아직 남았는데 진짜 마지막으로 빌릴 수 있을까?");
            ToggleOptionsUIStory("제발 이런 짓 좀 그만하면 안 되냐?", "나는 돈을 돌려받고 싶다고.");
            yield return WaitForChoice();
            AddMessage("아, 진짜 마지막이다. 맹세할게.");
            ToggleOptionsUIStory("네 말이 믿겨진다고 생각하니?", "너 같으면 빌려주겠니?");
            yield return WaitForChoice();
            AddMessage("진짜 마지막으로 한 번만 빌릴게.");
            yield return new WaitForSeconds(2f);
            AddMessage("진짜 진심이다.");
            yield return new WaitForSeconds(2f);
            if (currentMoney/2 >= 300) {
                bigMoney = 300;
            } else {
                bigMoney = currentMoney/2;
            }
            AddMessage($"{bigMoney} 만원이다.");
            ToggleOptionsUIStory("내가 미쳤지.", "아오 그냥!!!");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney-bigMoney);
            AddMessage("아, 진짜 진짜 고맙다. 꼭 이틀 뒤에 갚을게 ㅠㅠ");
            gameEvent.UpdateAllText();
        } else if (currentDay == 22) {
            ToggleOptionsUIStory("빨리 안 갚냐?","기상 기상!!!");
            yield return WaitForChoice();
            AddMessage("아, 진짜 너무 힘들다.");
            yield return new WaitForSeconds(2f);
            AddMessage("여기서도 재촉하고 저기서도 재촉하고.");
            yield return new WaitForSeconds(2f);
            AddMessage("그냥 신고당하고 감방이나 갈란다.");
            ToggleOptionsUIStory("야.", "뭐라고?");
            yield return WaitForChoice();
            AddMessage("(안 읽음 상태 유지)");
        }
    }

    private void ToggleOptionsUIStory(string str1, string str2) {
        ToggleOptionsUI();
        Button button1 = optionsUI.transform.Find("Button1").GetComponent<Button>();
        Button button2 = optionsUI.transform.Find("Button2").GetComponent<Button>();
        button1.onClick.AddListener(() => buttonPrint(button1.GetComponentInChildren<TMP_Text>().text));
        button2.onClick.AddListener(() => buttonPrint(button2.GetComponentInChildren<TMP_Text>().text));
        button1.GetComponentInChildren<TMP_Text>().text = $"{str1}";
        button2.GetComponentInChildren<TMP_Text>().text = $"{str2}";
    }

    private void buttonPrint(string content) {
        AddMyMessage(content);
        OnChoiceMade();
        ToggleOptionsUI();
    }
}