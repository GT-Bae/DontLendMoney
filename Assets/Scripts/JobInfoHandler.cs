/*
 * アルバイトの情報をPleayerPrefsに保存するクラス
 */

using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class JobInfoUI : MonoBehaviour
{
    public TMP_Text payText;
    public TMP_Text healthLossText;
    public TMP_Text Title;

    public void OnJobInfoClicked() {
        string payString = payText.text;
        int payNumber = int.Parse(Regex.Match(payString, @"\d+").Value);

        string healthLossString = healthLossText.text;
        int healthLossNumber = int.Parse(Regex.Match(healthLossString, @"\d+").Value);

        string ArbeitName = Title.text;

        PlayerPrefs.SetInt("PayValue", payNumber);
        PlayerPrefs.SetInt("HealthLossValue", healthLossNumber);
        PlayerPrefs.SetString("ArbeitName", ArbeitName);
        PlayerPrefs.Save();

        Debug.Log("アルバイト情報の保存が完了しました。");
    }
}