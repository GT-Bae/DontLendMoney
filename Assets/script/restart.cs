using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    public Button resetButton; // 초기화 버튼

    private void Start()
    {
        resetButton.onClick.AddListener(ResetProgress);
    }

    void ResetProgress()
    {
        // 진행 상태 초기화
        PlayerPrefs.DeleteKey("ChatEndIndex");
        PlayerPrefs.DeleteKey("Button1Clicked");
        PlayerPrefs.DeleteKey("Button2Clicked");

        // 게임 데이터 초기화
        PlayerPrefs.SetInt("CurrentDay", 28); // 기본 날짜
        PlayerPrefs.SetInt("CurrentHealth", 3); // 기본 체력
        PlayerPrefs.SetFloat("MyMoney", 10); // 플레이어 기본 소지금
        PlayerPrefs.SetInt("CurrentFriends", 0); // 기본 친구 수

        PlayerPrefs.Save();

        Debug.Log("게임 데이터가 초기화되었습니다.");
        SceneManager.LoadScene("myroomScenes");
    }
}
