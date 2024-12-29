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
        // GIMSEONGYUN 데이터 초기화
        PlayerPrefs.DeleteKey("ChatEndIndex"); // 채팅 종료 인덱스 초기화
        PlayerPrefs.DeleteKey("Button1Clicked"); // 버튼 1 클릭 초기화
        PlayerPrefs.DeleteKey("Button2Clicked"); // 버튼 2 클릭 초기화
        PlayerPrefs.DeleteKey("BorrowedDay"); // 빌린 날짜 초기화

        // 다른 캐릭터별 데이터 초기화
        ResetChatingData("BAKSIYUN");
        ResetChatingData("LEEJEONGMAN");
        ResetChatingData("GIMSIHYEON");
        ResetChatingData("BAKJONGHYEON");
        ResetChatingData("CHOIMINSU");
        ResetChatingData("GWONJANGMIN");
        ResetChatingData("JEONGDONGMIN");
        ResetChatingData("OHSEOYUN");
        ResetChatingData("GIMSUHWAN");
        ResetChatingData("BAKYEONJI");
        ResetChatingData("CHOIGYEONGMIN");

        PlayerPrefs.DeleteKey("CurrentDay"); // 현재 날짜 초기화
        PlayerPrefs.DeleteKey("MyMoney"); // 돈 초기화
        PlayerPrefs.DeleteKey("CurrentHealth"); // 체력 초기화
        PlayerPrefs.DeleteKey("SelectedIndices"); // ChatManager 버튼 선택 초기화
        PlayerPrefs.DeleteKey("DisplayedUsers"); // 표시된 사용자 목록 초기화
        PlayerPrefs.DeleteKey("PayValue"); // 알바 수당
        PlayerPrefs.DeleteKey("HealthLossValue"); // 알바 체력
        PlayerPrefs.DeleteKey("ArbeitName"); // 알바 이름

        // 기본 값 설정
        PlayerPrefs.SetInt("CurrentDay", 28); // 기본 날짜
        PlayerPrefs.SetFloat("MyMoney", 0); // 기본 돈
        PlayerPrefs.SetInt("CurrentHealth", 0); // 기본 체력

        // 첫 날 표시 플래그 설정
        PlayerPrefs.SetInt("NewDay", 1); // NewDay 플래그를 true로 설정

        PlayerPrefs.Save();

        Debug.Log("모든 데이터가 초기화되었습니다.");
        SceneManager.LoadScene("GameRoom");
    }

    void ResetChatingData(string characterPrefix)
    {
        PlayerPrefs.DeleteKey($"ChatEndIndex_{characterPrefix}"); // 채팅 종료 인덱스 초기화
        PlayerPrefs.DeleteKey($"Button1Clicked_{characterPrefix}"); // 버튼 1 클릭 초기화
        PlayerPrefs.DeleteKey($"Button2Clicked_{characterPrefix}"); // 버튼 2 클릭 초기화
        PlayerPrefs.DeleteKey($"BorrowedDay_{characterPrefix}"); // 빌린 날짜 초기화
    }
}