using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitleManager : MonoBehaviour
{
    public void LoadGameRoom()
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
                    resetScript.ResetValues();
                }
            }
            SceneManager.sceneLoaded -= OnGameRoomLoaded; // 이벤트 핸들러 제거
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 모드 종료
        #else
            Application.Quit(); // 애플리케이션 종료
        #endif
    }
}