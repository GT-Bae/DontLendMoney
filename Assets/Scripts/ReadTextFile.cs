using UnityEngine;
using System.IO;

public class ReadTextFile : MonoBehaviour
{
    void Start()
    {
        // Resources 폴더에서 텍스트 파일 읽기
        string filePath = "Assets/Texts/FriendsNameList.txt";
        
        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // 파일을 읽어서 텍스트로 변환
            string fileContents = File.ReadAllText(filePath);
            
            // 쉼표를 기준으로 split
            string[] splitContents = fileContents.Split(',');

            // 콘솔에 출력
            foreach (string item in splitContents)
            {
                Debug.Log(item);
            }
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다.");
        }
    }
}
