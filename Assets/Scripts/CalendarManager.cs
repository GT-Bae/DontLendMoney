/*
 * カレンダーの日付更新及び予定（Todo）の設定を制御するクラス
 */

using UnityEngine;
using TMPro;

public class CalendarManager : MonoBehaviour
{
    public TMP_Text[] dateTexts; // Date1~7のTMP_Text配列
    public TMP_Text[] todoTexts; // Todo1~7のTMP_Text配列

    public void UpdateDates() // カレンダーの日付を更新し、全ての表示に7日加算する
    {
        for (int i = 0; i < dateTexts.Length; i++)
        {
            if (int.TryParse(dateTexts[i].text, out int dateValue))
            {
                dateValue += 7;
                dateTexts[i].text = dateValue.ToString();
            }
            else
            {
                Debug.LogError("日付が整数ではありません。日付: " + dateTexts[i].text);
            }
        }
    }

    public void SetTodo(int index, string todo) // 指定されたインデックスのTodoテキストを設定する
    {
        if (index >= 0 && index < todoTexts.Length)
        {
            todoTexts[index].text = todo;
        }
        else
        {
            Debug.LogError($"インデックスが範囲外です。指定されたインデックス: {index}");
        }
    }
}