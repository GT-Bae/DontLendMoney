/*
 * ReportというPleayerPrefsを1に設定するクラス
 */

using UnityEngine;

public class DoReport : MonoBehaviour
{
    public void doReport() {
        PlayerPrefs.SetInt("Report",1);
        PlayerPrefs.Save();
    }
}