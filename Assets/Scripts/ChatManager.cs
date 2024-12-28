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

    private List<UserData> allUsers = new List<UserData>();
    private List<UserData> displayedUsers = new List<UserData>();
    private HashSet<int> usedIndices = new HashSet<int>(); // 이미 뽑힌 유저의 인덱스
    private const int usersPerDay = 3;

    private void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ReturnToMyRoom);
        }

        LoadUserData();

        // 기존 버튼 로드
        LoadDisplayedUsers();

        // NewDay 값이 1이면 새로운 버튼 추가
        if (PlayerPrefs.GetInt("NewDay", 0) == 1)
        {
            PlayerPrefs.SetInt("NewDay", 0); // Reset NewDay flag
            AddNewButtonsForDay();
        }
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

            allUsers = userList.items;
        }
        else
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다. 경로: Resources/{userDataFileName}.json");
        }
    }

    void AddNewButtonsForDay()
    {
        int addedCount = 0;

        while (addedCount < usersPerDay && usedIndices.Count < allUsers.Count)
        {
            int randomIndex = Random.Range(0, allUsers.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                usedIndices.Add(randomIndex);
                UserData newUser = allUsers[randomIndex];
                displayedUsers.Add(newUser);
                CreateChatButton(newUser);
                addedCount++;
            }
        }

        SaveDisplayedUsers();
    }

    void LoadDisplayedUsers()
    {
        string savedData = PlayerPrefs.GetString("DisplayedUsers", "");
        if (!string.IsNullOrEmpty(savedData))
        {
            UserDataList loadedData = JsonUtility.FromJson<UserDataList>(savedData);
            displayedUsers = loadedData.items;

            foreach (var user in displayedUsers)
            {
                CreateChatButton(user);
            }
        }
    }

    void SaveDisplayedUsers()
    {
        UserDataList dataToSave = new UserDataList { items = displayedUsers };
        string jsonData = JsonUtility.ToJson(dataToSave);
        PlayerPrefs.SetString("DisplayedUsers", jsonData);

        // 이미 사용된 인덱스 저장
        PlayerPrefs.SetString("UsedIndices", string.Join(",", usedIndices));
        PlayerPrefs.Save();
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
