/*
 * JobCompleteUIの文句を設定するクラス
 */
using UnityEngine;
using TMPro;

public class JobCompleteUI : MonoBehaviour
{
    public TMP_Text payText;
    public TMP_Text healthText;

    void Start() {
        int PayValue = PlayerPrefs.GetInt("PayValue", 0);
        int HealthLoss = PlayerPrefs.GetInt("HealthLossValue", 0);
        payText.text = "+" + PayValue + "千円";
        healthText.text = "体力 -" + HealthLoss;
    }
}
