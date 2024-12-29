using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class JobCheckUI : MonoBehaviour
{
    public TMP_Text arbeitNameText;

    void Start() {
        // PlayerPrefs에서 "ArbeitName" 값 가져오기
        string arbeitName = PlayerPrefs.GetString("ArbeitName", "DefaultName");

        // ArbeitName 텍스트 설정
        arbeitNameText.text = arbeitName;
    }

    public void CallPerformAlba() {
        // 이름으로 GameManager 오브젝트 찾기
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null) {
            GameEvent gameEvent = gameManagerObject.GetComponent<GameEvent>();
            if (gameEvent != null) {
                gameEvent.PerformAlba();
            } else {
                Debug.LogError("GameEvent component is missing on GameManager");
            }
        } else {
            Debug.LogError("GameManager object not found");
        }
    }
}