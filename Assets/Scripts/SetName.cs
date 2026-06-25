/*
 * プレイヤーのハンドルネームを設定及び保存するクラス
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetName : MonoBehaviour
{
    public InputField inputField;
    public TMP_Text displayText;

    public void SetNameData() {
        PlayerPrefs.SetString("Name", inputField.text);
        displayText.text = inputField.text;
        
        if (string.IsNullOrWhiteSpace(inputField.text)) {
            PlayerPrefs.SetString("Name", "ブランク");
            displayText.text = "ブランク";
        };        
    }
}