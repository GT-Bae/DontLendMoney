using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameEvent : MonoBehaviour
{
    public Text healthText; // 泥대  UI ㅽ
    public Text dayText; // 吏  UI ㅽ
    public Text mymoneytext; //   UI ㅽ
    public Text progressText;  // 紐⑺ 湲 吏   UI ㅽ
    public Text friendText; // 移援   UI ㅽ

    private const int maxHealth = 3; // 理 泥대
    private const int maxFriends = 0; // 理 移援 

    private void Start()
    {
        UpdateDayText();
        UpdateHealthText(PlayerPrefs.GetInt("CurrentHealth", maxHealth));
        UpdateMoneyText(PlayerPrefs.GetFloat("MyMoney", 10f));
        UpdateProgressText(PlayerPrefs.GetFloat("MyMoney", 10f));
        UpdateFriendText(PlayerPrefs.GetInt("CurrentFriends", 0));
    }

    private void SaveGameData()
    {
        PlayerPrefs.Save(); // 寃 곗댄 �
        Debug.Log("寃 곗댄곌 �λ듬.");
    }

    public void PerformAlba()
    {
        int currentHealth = PlayerPrefs.GetInt("CurrentHealth", maxHealth);
        float currentMoney = PlayerPrefs.GetFloat("MyMoney", 10f);

        if (currentHealth > 0)
        {
            currentHealth--;
            currentMoney += 10;

            PlayerPrefs.SetInt("CurrentHealth", currentHealth);
            PlayerPrefs.SetFloat("MyMoney", currentMoney);
            SaveGameData();

            UpdateHealthText(currentHealth);
            UpdateMoneyText(currentMoney);
            UpdateProgressText(currentMoney);
            Debug.Log("알바를 완료했습니다. 체력 -1, 돈 +10만 원");
        }
        else
        {
            Debug.Log("체력이 부족하여 알바를 할 수 없습니다.");
        }
    }

    public void PerformSleep()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 28); //  吏 媛�몄ㅺ린
        if (currentDay > 0) // 吏媛 ⑥쇰㈃
        {
            currentDay--; // 吏 媛
            PlayerPrefs.SetInt("CurrentDay", currentDay); // 吏 �

            // 濡  
            PlayerPrefs.SetInt("NewDay", 1); // NewDay 媛 1濡 ㅼ
            PlayerPrefs.SetInt("CurrentHealth", maxHealth); // 泥대μ 理媛쇰 蹂
            SaveGameData(); // 寃 곗댄 �

            UpdateDayText(); // 吏 ㅽ 곗댄
            UpdateHealthText(maxHealth); // 泥대 ㅽ 곗댄

            Debug.Log(" 怨 猷④ 吏媛듬. 吏 -1, 泥대 蹂, 濡  .");
        }
        else
        {
            Debug.Log(" 댁 吏媛 ⑥     듬.");
        }
    }

    private void UpdateHealthText(int health)
    {
        healthText.text = "泥대: " + health + " / " + maxHealth; // 泥대 ㅽ 곗댄
    }

    private void UpdateDayText()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 28); //  吏 媛�몄ㅺ린
        dayText.text = "D-" + currentDay; // 吏 ㅽ 곗댄
    }

    private void UpdateMoneyText(float money)
    {
        mymoneytext.text = "怨: " + money + "留"; //  ㅽ 곗댄
    }

    private void UpdateProgressText(float money)
    {
        progressText.text = "�ㅻ : " + money + "留"; // 吏  ㅽ 곗댄
    }

    private void UpdateFriendText(int friends)
    {
        friendText.text = "移援: " + friends + " / " + maxFriends; // 移援  ㅽ 곗댄
    }

    public void AddFriend()
    {
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0); //  移援  媛�몄ㅺ린

        if (currentFriends < maxFriends) // 理 移援 蹂대 �쇰㈃
        {
            currentFriends++; // 移援  利媛
            PlayerPrefs.SetInt("CurrentFriends", currentFriends); // 移援  �
            SaveGameData(); // 寃 곗댄 �

            UpdateFriendText(currentFriends); // 移援  ㅽ 곗댄
            Debug.Log("移援ш 異媛듬.");
        }
        else
        {
            Debug.Log("理 移援  ы듬.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData(); // 由ъ댁 醫猷  寃 곗댄 �
    }
}
