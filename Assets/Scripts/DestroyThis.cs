/*
 * このコンポーネントが付いたオブジェクトを削除
 */

using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public void destroyThis() {
        Destroy(gameObject);
    }
}