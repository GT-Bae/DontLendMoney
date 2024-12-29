using UnityEngine;

public class Reset : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ResetValues() {
        // 값 삭제
        PlayerPrefs.DeleteKey("SelectedIndices"); // ChatManager 버튼 선택
        PlayerPrefs.DeleteKey("DisplayedUsers"); // 표시된 사용자 목록
        PlayerPrefs.DeleteKey("PayValue"); // 알바 수당
        PlayerPrefs.DeleteKey("HealthLossValue"); // 알바 체력
        PlayerPrefs.DeleteKey("ArbeitName"); // 알바 이름

        // 기본 값 설정
        PlayerPrefs.SetInt("CurrentDay", 0); // 기본 날짜
        PlayerPrefs.SetFloat("MyMoney", 0); // 기본 돈
        PlayerPrefs.SetInt("CurrentHealth", 3); // 기본 체력
        PlayerPrefs.SetInt("CurrentFriends",0); // 기본 친구
        PlayerPrefs.Save();

        // 이름으로 GameManager 오브젝트 찾기
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null) {
            GameEvent gameEvent = gameManagerObject.GetComponent<GameEvent>();
            if (gameEvent != null) {
                gameEvent.UpdateAllText();
            } else {
                Debug.LogError("GameEvent component is missing on GameManager");
            }
        } else {
            Debug.LogError("GameManager object not found");
        }

        Debug.Log("모든 데이터가 초기화되었습니다.");
    }
}
