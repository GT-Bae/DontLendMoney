/*
 * JobCheckUIのテキストを設定するクラス
 */

using UnityEngine;
using TMPro;

public class JobCheckUI : MonoBehaviour
{
    public TMP_Text arbeitNameText;

    void Start() {
        string arbeitName = PlayerPrefs.GetString("ArbeitName", "DefaultName");
        arbeitNameText.text = arbeitName;
    }

    public void CallPerformAlba() {
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null) {
            GameEvent gameEvent = gameManagerObject.GetComponent<GameEvent>();
            if (gameEvent != null) {
                gameEvent.PerformAlba();
            } else {
                Debug.LogError("GameManagerにGameEventコンポーネントがありません。");
                throw new MissingComponentException();
            }
        } else {
            Debug.LogError("GameManagerオブジェクトがありません。");
        }
    }
}