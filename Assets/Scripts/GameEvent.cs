using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameEvent : MonoBehaviour
{
    public Text healthText; // 체력 표시 UI 텍스트
    public Text dayText; // 날짜 표시 UI 텍스트
    public Text mymoneytext; // 돈 표시 UI 텍스트
    public Text progressText;  // 목표 금액 진행 상태 표시 UI 텍스트
    public Text friendText; // 친구 수 표시 UI 텍스트

    private const int maxHealth = 3; // 최대 체력
    private const int maxFriends = 0; // 최대 친구 수

    private void Start()
    {
        UpdateDayText(); // 날짜 업데이트
        UpdateHealthText(PlayerPrefs.GetInt("CurrentHealth", maxHealth)); // 체력 업데이트
        UpdateMoneyText(PlayerPrefs.GetFloat("MyMoney", 10f)); // 돈 업데이트
        UpdateProgressText(PlayerPrefs.GetFloat("MyMoney", 10f)); // 진행 상황 업데이트
        UpdateFriendText(PlayerPrefs.GetInt("CurrentFriends", 0)); // 친구 수 업데이트
    }

    private void SaveGameData()
    {
        PlayerPrefs.Save(); // 게임 데이터 저장
        Debug.Log("게임 데이터가 저장되었습니다.");
    }

    public void PerformAlba()
    {
        int currentHealth = PlayerPrefs.GetInt("CurrentHealth", maxHealth); // 현재 체력 가져오기
        float currentMoney = PlayerPrefs.GetFloat("MyMoney", 10f); // 현재 돈 가져오기

        if (currentHealth > 0) // 체력이 남아있으면
        {
            currentHealth--; // 체력 감소
            currentMoney += 10; // 돈 증가

            PlayerPrefs.SetInt("CurrentHealth", currentHealth); // 체력 저장
            PlayerPrefs.SetFloat("MyMoney", currentMoney); // 돈 저장
            SaveGameData(); // 게임 데이터 저장

            UpdateHealthText(currentHealth); // 체력 텍스트 업데이트
            UpdateMoneyText(currentMoney); // 돈 텍스트 업데이트
            UpdateProgressText(currentMoney); // 진행 상황 업데이트

            Debug.Log("아르바이트를 했습니다. 체력 -1, 돈 +10");
        }
        else
        {
            Debug.Log("체력이 부족하여 아르바이트를 할 수 없습니다.");
        }
    }

    public void PerformSleep()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 28); // 현재 날짜 가져오기
        if (currentDay > 0) // 날짜가 남아있으면
        {
            currentDay--; // 날짜 감소
            PlayerPrefs.SetInt("CurrentDay", currentDay); // 날짜 저장

            // 새로운 날 시작
            PlayerPrefs.SetInt("NewDay", 1); // NewDay 값을 1로 설정
            PlayerPrefs.SetInt("CurrentHealth", maxHealth); // 체력을 최대값으로 회복
            SaveGameData(); // 게임 데이터 저장

            UpdateDayText(); // 날짜 텍스트 업데이트
            UpdateHealthText(maxHealth); // 체력 텍스트 업데이트

            Debug.Log("잠을 자고 하루가 지나갔습니다. 날짜 -1, 체력 회복, 새로운 날 시작.");
        }
        else
        {
            Debug.Log("더 이상 날짜가 남지 않아 잠을 잘 수 없습니다.");
        }
    }

    private void UpdateHealthText(int health)
    {
        healthText.text = "체력: " + health + " / " + maxHealth; // 체력 텍스트 업데이트
    }

    private void UpdateDayText()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 28); // 현재 날짜 가져오기
        dayText.text = "D-" + currentDay; // 날짜 텍스트 업데이트
    }

    private void UpdateMoneyText(float money)
    {
        mymoneytext.text = "잔고: " + money + "만원"; // 돈 텍스트 업데이트
    }

    private void UpdateProgressText(float money)
    {
        progressText.text = "돌려받은 돈: " + money + "만원"; // 진행 상태 텍스트 업데이트
    }

    private void UpdateFriendText(int friends)
    {
        friendText.text = "친구: " + friends + " / " + maxFriends; // 친구 수 텍스트 업데이트
    }

    public void AddFriend()
    {
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0); // 현재 친구 수 가져오기

        if (currentFriends < maxFriends) // 최대 친구 수보다 적으면
        {
            currentFriends++; // 친구 수 증가
            PlayerPrefs.SetInt("CurrentFriends", currentFriends); // 친구 수 저장
            SaveGameData(); // 게임 데이터 저장

            UpdateFriendText(currentFriends); // 친구 수 텍스트 업데이트
            Debug.Log("친구가 추가되었습니다.");
        }
        else
        {
            Debug.Log("최대 친구 수에 도달했습니다.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData(); // 애플리케이션 종료 시 게임 데이터 저장
    }
}
