using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class JobInfoUI : MonoBehaviour
{
    public TMP_Text payText;
    public TMP_Text healthLossText;

    public void OnJobInfoClicked() {
        // Pay 텍스트에서 숫자 추출
        string payString = payText.text;
        int payNumber = int.Parse(Regex.Match(payString, @"\d+").Value);

        // HealthLoss 텍스트에서 숫자 추출
        string healthLossString = healthLossText.text;
        int healthLossNumber = int.Parse(Regex.Match(healthLossString, @"\d+").Value);

         // PlayerPrefs에 값 저장
        PlayerPrefs.SetInt("PayValue", payNumber);
        PlayerPrefs.SetInt("HealthLossValue", healthLossNumber);
        PlayerPrefs.Save();

        Debug.Log("알바 값 저장 완료");
    }
}