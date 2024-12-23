using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UserData
{
    public string profile;
    public string name;
    public string message;
    public string sceneName;
}

[System.Serializable]
public class UserDataList
{
    public List<UserData> items;
}

public class ChatManager : MonoBehaviour
{
    public GameObject chatButtonPrefab;
    public Transform contentArea;
    public string userDataFileName = "userData";
    public Button exitButton;

    private void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ReturnToMyRoom);
        }

        LoadUserData();
    }

    void ReturnToMyRoom()
    {
        Debug.Log("나가기 버튼 클릭: GameRoom 이동");
        SceneManager.LoadScene("GameRoom");
    }

    void LoadUserData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(userDataFileName);
        if (textAsset != null)
        {
            string jsonData = textAsset.text;

            if (!jsonData.TrimStart().StartsWith("{"))
            {
                jsonData = "{\"items\":" + jsonData + "}";
            }

            UserDataList userList = JsonUtility.FromJson<UserDataList>(jsonData);

            foreach (var user in userList.items)
            {
                CreateChatButton(user);
            }
        }
        else
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다. 경로: Resources/{userDataFileName}.json");
        }
    }

    void CreateChatButton(UserData user)
    {
        GameObject chatButton = Instantiate(chatButtonPrefab, contentArea);

        Text[] texts = chatButton.GetComponentsInChildren<Text>();
        if (texts.Length >= 2)
        {
            texts[0].text = user.name;
            texts[1].text = user.message;
        }
        else
        {
            Debug.LogError("버튼 프리팹에 필요한 Text 컴포넌트가 설정되지 않았습니다.");
        }

        Image profileImage = chatButton.GetComponentInChildren<Image>();
        if (profileImage != null)
        {
            Sprite profileSprite = Resources.Load<Sprite>(user.profile);
            if (profileSprite != null)
            {
                profileImage.sprite = profileSprite;
            }
            else
            {
                Debug.LogWarning($"프로필 이미지를 찾을 수 없습니다. 경로: Resources/{user.profile}");
            }
        }
        else
        {
            Debug.LogError("버튼 프리팹에 Image 컴포넌트가 설정되지 않았습니다.");
        }

        Button button = chatButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnButtonClick(user));
        }
        else
        {
            Debug.LogError("버튼 프리팹에 Button 컴포넌트가 설정되지 않았습니다.");
        }
    }

    void OnButtonClick(UserData user)
    {
        if (!string.IsNullOrEmpty(user.sceneName))
        {
            Debug.Log($"씬 로딩: {user.sceneName}");
            SceneManager.LoadScene(user.sceneName);
        }
        else
        {
            Debug.LogWarning("씬 이름이 비어 있습니다.");
        }
    }
}
