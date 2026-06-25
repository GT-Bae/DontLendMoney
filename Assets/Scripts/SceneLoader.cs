/*
 * ボタンのOnClickイベントから呼び出され、シーン遷移を行うクラス
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public void SceneLoad()
    {
        SceneManager.LoadScene(sceneName);
    }
}
