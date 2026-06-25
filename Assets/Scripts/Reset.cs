/*
 * PlayerPrefsデータを全てリセットするクラス
 */

using UnityEngine;

public class Reset : MonoBehaviour
{
    public void ResetValues() {    
        PlayerPrefs.SetInt("CurrentDay", 0);        //現在の日付
        PlayerPrefs.SetInt("MyMoney", 1000);        //所持金
        PlayerPrefs.SetInt("CurrentHealth", 3);     //現在体力
        PlayerPrefs.SetInt("CurrentFriends", 0);    //現在の友達数
        PlayerPrefs.SetInt("MaxHealth", 3);         //最大体力
        PlayerPrefs.SetInt("MaxFriends", 0);        //最大友達数
        PlayerPrefs.SetInt("hasLoan", 0);           //現在の借金フラグ（0:なし, 1:あり）
        PlayerPrefs.SetInt("Loan", 0);              //借金履歴フラグ （エンディング分岐用：一度でも借金したら1のまま維持）
        PlayerPrefs.SetInt("Report", 0);            //通報フラグ（0:なし, 1:あり）
        PlayerPrefs.SetInt("Recovery", 0);          //返済した金額
        PlayerPrefs.Save();       

        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null) {
            GameEvent gameEvent = gameManagerObject.GetComponent<GameEvent>();
            if (gameEvent != null) {
                gameEvent.UpdateAllText();
            } else {
                Debug.LogError("GameManagerにGameEventコンポーネントがアタッチされていません。");
                throw new MissingComponentException();
            }
        } else {
            Debug.LogError("GameManagerオブジェクトが見つかりません。");
        }

        Debug.Log("全てのデータがリセットされました。");
    }
}
