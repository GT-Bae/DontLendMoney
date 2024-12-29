using UnityEngine;
using TMPro;

public class JobCompleteUI : MonoBehaviour
{
    public TMP_Text pay;
    public TMP_Text health;

    void Start() {
        // PlayerPrefs에서 값 가져오기
        int PayValue = PlayerPrefs.GetInt("PayValue", 0);
        int HealthLoss = PlayerPrefs.GetInt("HealthLossValue", 0);
        
        // 텍스트 설정
        pay.text = "+" + PayValue + "만원";
        health.text = "체력 -" + HealthLoss;
    }
}
