using UnityEngine;

public class Reset : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ResetValues() {
        // 기본 값 설정
        PlayerPrefs.SetInt("CurrentDay", 0); // 기본 날짜
        PlayerPrefs.SetInt("MyMoney", 1000); // 기본 돈
        PlayerPrefs.SetInt("CurrentHealth", 3); // 기본 체력
        PlayerPrefs.SetInt("CurrentFriends",0); // 기본 친구
        PlayerPrefs.SetInt("MaxHealth",3);
        PlayerPrefs.SetInt("MaxFriends",0);
        PlayerPrefs.SetInt("hasLoan",0);
        PlayerPrefs.SetInt("Loan",0);
        PlayerPrefs.SetInt("Report",0);
        PlayerPrefs.SetInt("Recovery",0);
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
