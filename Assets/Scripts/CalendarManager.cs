using UnityEngine;
using TMPro;

public class CalendarManager : MonoBehaviour
{
    public TMP_Text[] dateTexts; // Date1~7의 TMP_Text 배열
    public TMP_Text[] todoTexts; // Todo1~7의 TMP_Text 배열

    public void UpdateDates() // 7더하기
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
                Debug.LogError("날짜가 정수가 아닙니다: " + dateTexts[i].text);
            }
        }
    }

    public void SetTodo(int index, string todo)
    {
        if (index >= 0 && index < todoTexts.Length)
        {
            todoTexts[index].text = todo;
        }
    }
}