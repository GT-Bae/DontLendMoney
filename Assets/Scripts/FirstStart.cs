/*
 * タイトル画面からゲームを開始する際、データのリセットと初期イベントを行うクラス
 */
 
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstStart : MonoBehaviour
{
    public void FirstLoadGameRoom()
    {
        SceneManager.LoadScene("GameRoom");
        SceneManager.sceneLoaded += OnGameRoomLoaded;
    }

    private void OnGameRoomLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameRoom")
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                Reset resetScript = gameManager.GetComponent<Reset>();
                if (resetScript != null)
                {
                    resetScript.ResetValues(); //PleayerPrefsを全てリセット
                }
                else
                {
                    Debug.LogError("gameManagerにresetコンポーネントがアタッチされていません。");
                }
                GameEvent gameEvent = gameManager.GetComponent<GameEvent>();
                if (gameEvent != null)
                {
                    gameEvent.OpeningUIActive(); //初期イベント
                }
                else 
                {
                    Debug.LogError("gameManagerにgameEventコンポーネントがアタッチされていません。");
                }
            }
            SceneManager.sceneLoaded -= OnGameRoomLoaded;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //エディタからプレイモード終了
        #else
            Application.Quit(); // アプリ終了
        #endif
    }
}