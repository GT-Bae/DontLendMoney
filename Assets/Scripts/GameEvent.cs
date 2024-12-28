using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameEvent : MonoBehaviour
{
    public Text healthText; // 체력을 표시할 UI 텍스트
    public Text dayText; // 날짜를 표시할 UI 텍스트
    public Text mymoneytext; // 플레이어 소지금 텍스트
    public Text progressText;  // 목표 금액 달성 진행 상황 표시 텍스트
    public Text friendText; // 친구 수 텍스트

    public Button albaButton; // 알바 버튼
    public Button sleepButton; // 휴식 버튼
    public Button phoneButton; // 핸드폰 버튼

    private const int maxHealth = 3;
    private const int maxFriends = 60;
    private const float targetAmount = 1000; // 목표 금액

    private void Start()
    {
        UpdateDayText();
        UpdateHealthText(PlayerPrefs.GetInt("CurrentHealth", maxHealth));
        UpdateMoneyText(PlayerPrefs.GetFloat("MyMoney", 10f));
        UpdateProgressText(PlayerPrefs.GetFloat("MyMoney", 10f));
        UpdateFriendText(PlayerPrefs.GetInt("CurrentFriends", 0));

        // 버튼 클릭 이벤트 등록
        //albaButton.onClick.AddListener(PerformAlba);
        sleepButton.onClick.AddListener(PerformSleep);
        phoneButton.onClick.AddListener(OpenPhone);
    }

    private void SaveGameData()
    {
        PlayerPrefs.Save();
        Debug.Log("게임 데이터가 저장되었습니다.");
    }

    //public void PerformAlba()
    //{
    //    int currentHealth = PlayerPrefs.GetInt("CurrentHealth", maxHealth);
    //    float currentMoney = PlayerPrefs.GetFloat("MyMoney", 10f);

    //    if (currentHealth > 0)
    //    {
    //        currentHealth--;
    //        currentMoney += 10;

    //        PlayerPrefs.SetInt("CurrentHealth", currentHealth);
    //        PlayerPrefs.SetFloat("MyMoney", currentMoney);
    //        SaveGameData();

    //        UpdateHealthText(currentHealth);
    //        UpdateMoneyText(currentMoney);
    //        UpdateProgressText(currentMoney);

    //        Debug.Log("알바를 완료했습니다. 체력 -1, 돈 +10만 원");
    //    }
    //    else
    //    {
    //        Debug.Log("체력이 부족하여 알바를 할 수 없습니다.");
    //    }
    //}

    public void PerformSleep()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 28);
        if (currentDay > 0)
        {
            currentDay--;
            PlayerPrefs.SetInt("CurrentDay", currentDay);

            // 새로운 날 시작
            PlayerPrefs.SetInt("NewDay", 1); // NewDay 값을 1로 설정
            PlayerPrefs.SetInt("CurrentHealth", maxHealth); // 체력 회복
            SaveGameData();

            UpdateDayText();
            UpdateHealthText(maxHealth);

            Debug.Log("휴식을 취했습니다. 날짜 -1, 체력 완전히 회복. 새로운 날이 시작되었습니다.");
        }
        else
        {
            Debug.Log("날짜가 더 이상 줄어들 수 없습니다.");
        }
    }

    private void UpdateHealthText(int health)
    {
        healthText.text = "체력: " + health + " / " + maxHealth;
    }

    private void UpdateDayText()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 28);
        dayText.text = "Day -" + currentDay + "D";
    }

    private void UpdateMoneyText(float money)
    {
        mymoneytext.text = money + "만원";
    }

    private void UpdateProgressText(float money)
    {
        progressText.text = money + "만원 / " + targetAmount + "만원";
    }

    private void UpdateFriendText(int friends)
    {
        friendText.text = "내 친구: " + friends + " / " + maxFriends;
    }

    public void AddFriend()
    {
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0);

        if (currentFriends < maxFriends)
        {
            currentFriends++;
            PlayerPrefs.SetInt("CurrentFriends", currentFriends);
            SaveGameData();

            UpdateFriendText(currentFriends);
            Debug.Log("친구를 추가했습니다.");
        }
        else
        {
            Debug.Log("최대 친구 수를 도달했습니다.");
        }
    }

    public void OpenPhone()
    {
        SaveGameData();
        SceneManager.LoadScene("masaage");
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
