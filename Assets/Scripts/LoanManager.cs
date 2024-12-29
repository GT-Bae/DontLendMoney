using UnityEngine;
using UnityEngine.UI;

public class LoanManager : MonoBehaviour
{
    public GameObject loanUI; // 대출 UI
    public GameObject repayUI; // 대출 갚기 UI
    public Button adButton; // AD 버튼
    public Button loanButton; // 대출받기 버튼
    public Button repayButton; // 대출갚기 버튼

    public bool hasLoan = false; // 대출 상태
    private GameEvent gameEvent; // 게임 이벤트 스크립트

    void Start()
    {
        gameEvent = GetComponent<GameEvent>();
        // 초기 UI 설정
        loanUI.SetActive(false);
        repayUI.SetActive(false);

        // AD 버튼 클릭 이벤트 설정
        adButton.onClick.AddListener(ToggleLoanUI);

        // 대출받기 버튼 클릭 이벤트 설정
        loanButton.onClick.AddListener(TakeLoan);

        // 대출갚기 버튼 클릭 이벤트 설정
        repayButton.onClick.AddListener(RepayLoan);
    }

    void ToggleLoanUI()
    {
        if (hasLoan)
        {
            // 대출이 있는 경우 대출 갚기 UI 표시
            loanUI.SetActive(false);
            repayUI.SetActive(true);
        }
        else
        {
            // 대출이 없는 경우 대출 UI 표시
            loanUI.SetActive(true);
            repayUI.SetActive(false);
        }
    }

    void TakeLoan()
    {
        // 대출받기 로직
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        currentMoney += 300;
        PlayerPrefs.SetInt("MyMoney", currentMoney);
        PlayerPrefs.SetInt("hasLoan",1);
        PlayerPrefs.SetInt("Loan",1);
        gameEvent.UpdateAllText();
        PlayerPrefs.Save();
        hasLoan = true;
        loanUI.SetActive(false);
        Debug.Log("대출을 받았습니다.");
    }

    void RepayLoan()
    {
        // 대출갚기 로직
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        if (currentMoney >= 300) {
            currentMoney -= 300;
            PlayerPrefs.SetInt("MyMoney", currentMoney);
            gameEvent.UpdateAllText();
            PlayerPrefs.SetInt("hasLoan",0);
            PlayerPrefs.Save();
            hasLoan = false;
            repayUI.SetActive(false);
            Debug.Log("대출을 갚았습니다.");
        } else {
            Debug.Log("돈이 부족합니다");
        }
    }
}