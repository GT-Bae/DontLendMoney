/*
 * セーブデータ（日付）の有無に応じて「続きから」ボタンの有効・無効を切り替えるクラス
 */

using UnityEngine;
using UnityEngine.UI;

public class ContinueCheck : MonoBehaviour
{
    public Button yourButton;

    private void Start() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 0);
        if (currentDay > 0)
        {
            yourButton.GetComponent<Image>().color = Color.white;
            yourButton.interactable = true;
        }
        else
        {
            yourButton.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            yourButton.interactable = false;
        }
    }
}
    
