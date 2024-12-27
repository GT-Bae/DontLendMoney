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
        PlayerPrefs.DeleteKey("ChatEndIndex"); // 진행 상태 초기화
        PlayerPrefs.DeleteKey("Button1Clicked"); // 버튼 1 상태 초기화
        PlayerPrefs.DeleteKey("Button2Clicked"); // 버튼 2 상태 초기화
        PlayerPrefs.DeleteKey("BorrowedDay"); // 빌린 날짜 초기화

        // 기타 캐릭터별 데이터 초기화
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

        PlayerPrefs.DeleteKey("CurrentDay"); // 날짜 초기화
        PlayerPrefs.DeleteKey("MyMoney"); // 금액 초기화
        PlayerPrefs.DeleteKey("CurrentHealth"); // 체력 초기화
        PlayerPrefs.DeleteKey("SelectedIndices"); // ChatManager 버튼 상태 초기화
        PlayerPrefs.DeleteKey("DisplayedUsers"); // 표시된 사용자 상태 초기화

        // 기본 값 설정
        PlayerPrefs.SetInt("CurrentDay", 28); // 기본 날짜
        PlayerPrefs.SetFloat("MyMoney", 300f); // 기본 금액
        PlayerPrefs.SetInt("CurrentHealth", 30); // 기본 체력

        // 첫 날 실행 플래그 설정
        PlayerPrefs.SetInt("NewDay", 1); // NewDay 플래그를 true로 설정

        PlayerPrefs.Save();

        Debug.Log("모든 데이터가 초기화되었습니다.");
        SceneManager.LoadScene("GameRoom");
    }

    void ResetChatingData(string characterPrefix)
    {
        PlayerPrefs.DeleteKey($"ChatEndIndex_{characterPrefix}"); // 진행 상태 초기화
        PlayerPrefs.DeleteKey($"Button1Clicked_{characterPrefix}"); // 버튼 1 상태 초기화
        PlayerPrefs.DeleteKey($"Button2Clicked_{characterPrefix}"); // 버튼 2 상태 초기화
        PlayerPrefs.DeleteKey($"BorrowedDay_{characterPrefix}"); // 빌린 날짜 초기화
    }
}
