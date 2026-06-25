/* 
 * オブジェクトを生成するクラス
 */

using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    GameObject spawnedInstance;
    public void spawnSomething() {
        spawnedInstance = Instantiate(targetPrefab);
    }
}