using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public void IncreseHealth() {
        // 이름으로 GameManager 오브젝트 찾기
        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null) {
            GameEvent gameEvent = gameManagerObject.GetComponent<GameEvent>();
            if (gameEvent != null) {
                gameEvent.PerformSleep();
            } else {
                Debug.LogError("GameEvent component is missing on GameManager");
            }
        } else {
            Debug.LogError("GameManager object not found");
        }
    }
}