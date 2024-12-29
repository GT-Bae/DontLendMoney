using UnityEngine;

public class InstateSomething : MonoBehaviour
{
    public GameObject something;
    GameObject newPrefab;
    public void instateSomething() {
        // 프리팹 생성
        newPrefab = Instantiate(something);
    }
}