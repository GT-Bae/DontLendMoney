using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections.Generic;

public class GameEvent : MonoBehaviour
{
    public GameObject firstUI; // 처음 실행시 활성화할 오브젝트트
    public GameObject jobComplete; // 알바 완료 시 생성할 프리팹
    public GameObject jobLowHealth; // 체력 부족 시 생성할 프리팹
    public Text healthText; // 체력 UI 텍스트
    public Text dayText; // 현재 날짜 UI 텍스트
    public Text mymoneytext; // 내 돈 UI 텍스트
    public Text recoveryText;  // 진행 상황 UI 텍스트
    public Text friendText; // 친구 수 UI 텍스트
    public AudioSource audioSource; // AudioSource 컴포넌트를 연결
    public List<AudioClip> audioClips; // 여러 AudioClip을 저장할 리스트
    /*** 오디오
    0: 돈소리
    1: 경고소리
    2: 잠
    3: 침대 효과음
    4: 엔딩
    ***/

    public AudioSource specificAudioSource; // BGM 틀어주는 오브젝트
    private const int maxHealth = 3; // 최대 체력
    private const int maxFriends = 0; // 최대 친구 수

    private ArbeitPositioner arbeitPositioner; // 알바 설정 스크립트
    private BedtoSleep bedtoSleep; // 수면 효과 스크립트
    private GotoEnding gotoEnding; // 엔딩 스크립트
    private CalendarManager calendarManager; // 엔딩 스크립트
    private void Start()
    {
        // 같은 오브젝트에 있는 ArbeitPositioner 컴포넌트를 가져옴
        arbeitPositioner = GetComponent<ArbeitPositioner>();
        bedtoSleep = GetComponent<BedtoSleep>();
        gotoEnding = GetComponent<GotoEnding>();
        calendarManager = GetComponent<CalendarManager>();
        UpdateDayText();
        UpdateHealthText(PlayerPrefs.GetInt("CurrentHealth", maxHealth));
        UpdateMoneyText(PlayerPrefs.GetInt("MyMoney", 0));
        UpdateRecoveryText(PlayerPrefs.GetInt("Recovery", 0));
        UpdateFriendText(PlayerPrefs.GetInt("CurrentFriends", 0));
    }

    public void UpdateAllText() { //모든 UI 업데이트
        UpdateDayText();
        UpdateHealthText(PlayerPrefs.GetInt("CurrentHealth", maxHealth));
        UpdateMoneyText(PlayerPrefs.GetInt("MyMoney", 0));
        UpdateRecoveryText(PlayerPrefs.GetInt("Recovery", 0));
        UpdateFriendText(PlayerPrefs.GetInt("CurrentFriends", 0));
    }

    private void SaveGameData()
    {
        PlayerPrefs.Save(); // 게임 데이터를 저장
        Debug.Log("게임 데이터가 저장되었습니다.");
    }

    /*** ---------------테스트용----------------- ***/
    public void TestHealthIncrease() {
        PlayerPrefs.SetInt("CurrentHealth", maxHealth);
        UpdateHealthText(maxHealth); // 체력 UI 업데이트
    }

    /*** 알바 ***/
    public void PerformAlba()
    {
        int currentHealth = PlayerPrefs.GetInt("CurrentHealth", maxHealth);
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        int payValue = PlayerPrefs.GetInt("PayValue", 0);
        int healthLossValue = PlayerPrefs.GetInt("HealthLossValue", 0);
        if ((currentHealth - healthLossValue) >= 0)
        {
            currentHealth -= healthLossValue;
            currentMoney += payValue;

            PlayerPrefs.SetInt("CurrentHealth", currentHealth);
            PlayerPrefs.SetInt("MyMoney", currentMoney);
            SaveGameData();

            UpdateHealthText(currentHealth);
            UpdateMoneyText(currentMoney);
            Debug.Log("알바를 완료했습니다. 체력 -" + healthLossValue + ", 돈 +" + payValue);

            PlayAudio(0);
            Instantiate(jobComplete);
        }
        else
        {
            PlayAudio(1);
            Instantiate(jobLowHealth);
            Debug.Log("체력이 부족하여 알바를 할 수 없습니다.");
        }
    }

    /*** 수면 ***/
    public void PerformSleep()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1); // 현재 날짜 가져오기
        if (currentDay < 28) // 마지막날이면 엔딩으로
        {
            currentDay++; // 날짜 증가
            PlayerPrefs.SetInt("CurrentDay", currentDay); // 날짜 저장
            PlayerPrefs.SetInt("CurrentHealth", maxHealth); // 최대 체력으로 복원
            //PlayerPrefs.SetInt("NewDay", 1); // NewDay 값을 1로 설정

            SaveGameData(); // 게임 데이터 저장
            UpdateDayText(); // 날짜 UI 업데이트
            UpdateHealthText(maxHealth); // 체력 UI 업데이트
            arbeitPositioner.DailyArbeitPositioner(); // 알바 랜덤 돌림

            bedtoSleep.FadeOutWithMessage(); // 수면 효과

            PlayAudio(2);
            
            // 달력 업데이트
            switch (currentDay) {
                case 8:
                    calendarManager.UpdateDates();
                    calendarManager.SetTodo(2,"생활비\n-30만원");
                    break;
                case 15:
                    calendarManager.UpdateDates();
                    calendarManager.SetTodo(2,"");
                    calendarManager.SetTodo(3,"월세\n-50만원");
                    break;
                case 22:
                    calendarManager.UpdateDates();
                    calendarManager.SetTodo(3,"");
                    break;
            }
        }
        else
        {
            specificAudioSource.Stop();
            PlayAudio(4);
            gotoEnding.endSetting();
            Debug.Log("엔딩으로 가기");
        }
    }

    /*** 회수금 ***/
    public void ReceiveRecovery() {
        int currentRecovery = PlayerPrefs.GetInt("Recovery", 0); // 현재 회수금
        int returnAmount = PlayerPrefs.GetInt("Return", 0); // 친구가 주는 회수금

        currentRecovery += returnAmount; // 회수금에 더하기
        PlayerPrefs.SetInt("Recovery", currentRecovery); // 회수금 저장
        SaveGameData(); // 게임 데이터 저장

        UpdateRecoveryText(currentRecovery);

        Debug.Log("회수금이 업데이트되었습니다: " + currentRecovery);
    }

    /*** 친구 ***/
    public void AddFriend()
    {
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0); // 현재 친구 수 가져오기

        if (currentFriends < maxFriends) // 최대 친구 수 미달 시 친구 추가
        {
            currentFriends++; // 친구 수 증가
            PlayerPrefs.SetInt("CurrentFriends", currentFriends); // 친구 수 저장
            SaveGameData(); // 게임 데이터 저장

            UpdateFriendText(currentFriends); // 친구 수 UI 업데이트
            Debug.Log("친구가 추가되었습니다.");
        }
        else
        {
            Debug.Log("최대 친구 수를 초과할 수 없습니다.");
        }
    }

    private void UpdateHealthText(int health)
    {
        healthText.text = "체력: " + health + " / " + maxHealth; // 체력 UI 업데이트
    }

    private void UpdateDayText()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1); // 현재 날짜 가져오기
        dayText.text = currentDay + "일"; // 날짜 UI 업데이트
    }

    private void UpdateMoneyText(int money)
    {
        mymoneytext.text = "잔고: " + money + "만원"; // 내 돈 UI 업데이트
    }

    private void UpdateRecoveryText(int money)
    {
        recoveryText.text = "회수금: " + money + "만원"; // 진행 상황 UI 업데이트
    }

    private void UpdateFriendText(int friends)
    {
        friendText.text = "친구: " + friends + " / " + maxFriends; // 친구 수 UI 업데이트
    }

    private void OnApplicationQuit()
    {
        SaveGameData(); // 애플리케이션 종료 시 게임 데이터 저장
    }

    public void FirstUIActive() {
        firstUI.SetActive(true);
    }

    public void PlayAudio(int index) // 오디오 재생
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
            Debug.Log("오디오가 재생되었습니다: " + audioClips[index].name);
        }
        else
        {
            Debug.LogError("잘못된 인덱스입니다: " + index);
        }
    }
}
