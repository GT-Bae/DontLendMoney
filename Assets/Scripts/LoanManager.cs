/*
 * 借金の借入と返済を制御するクラス
 */

using UnityEngine;
using UnityEngine.UI;

public class LoanManager : MonoBehaviour
{
    public GameObject loanUI;
    public GameObject repayUI;
    public Button adButton;
    public Button loanButton;
    public Button repayButton;

    public bool hasLoan = false;
    private GameEvent gameEvent;

    void Start()
    {
        gameEvent = GetComponent<GameEvent>();
        loanUI.SetActive(false);
        repayUI.SetActive(false);

        adButton.onClick.AddListener(ToggleLoanUI);
        loanButton.onClick.AddListener(TakeLoan);
        repayButton.onClick.AddListener(RepayLoan);
    }

    void ToggleLoanUI()
    {
        if (hasLoan) //借金がある場合、返済UIを表示
        {
            loanUI.SetActive(false);
            repayUI.SetActive(true);
        }
        else //借金がない場合、借入UIを表示
        {
            loanUI.SetActive(true);
            repayUI.SetActive(false);
        }
    }

    void TakeLoan()
    {
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        currentMoney += 300;
        PlayerPrefs.SetInt("MyMoney", currentMoney);
        PlayerPrefs.SetInt("hasLoan",1);
        PlayerPrefs.SetInt("Loan",1);
        gameEvent.UpdateAllText();
        PlayerPrefs.Save();
        hasLoan = true;
        loanUI.SetActive(false);
        Debug.Log("ローンを実行しました。");
    }

    void RepayLoan()
    {
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        if (currentMoney >= 300) {
            currentMoney -= 300;
            PlayerPrefs.SetInt("MyMoney", currentMoney);
            gameEvent.UpdateAllText();
            PlayerPrefs.SetInt("hasLoan",0);
            PlayerPrefs.Save();
            hasLoan = false;
            repayUI.SetActive(false);
            Debug.Log("ローンの返済が完了しました。");
        } else {
            Debug.Log("所持金が足りません。");
        }
    }
}