using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용을 위한 네임스페이스

public class NameListPositioner : MonoBehaviour
{
    public Sprite[] profileImages; // 프로필 이미지 배열
    public List<string> staticNameList = new List<string>
    {"악마", "김민준", "고훈이"};
    public List<string> randomNameList = new List<string>
    {
        "동동수", "각티수", "마우수", "기보드", "마이클", "오이지", "수정석", "직구본",
        "대불암", "곽이손", "황꺽정", "김필수", "이우나", "박거세", "최면식", "정리왕",
        "강악진", "포도륙", "조조", "윤와사", "장티푼", "임영황", "오소박", "이발석",
        "황니터", "안릿지", "송스크", "홍달무", "유희롱", "전퓨즈", "고음양", "문자바",
        "붕객체", "양안로", "손픽아", "배지타", "조유테", "백송프", "허영굼", "남설구",
        "심밧드", "노비타", "하필드", "곽필승", "성씨샵", "차바코", "주로잉", "우스타",
        "구한말", "사만루", "나파엘", "민리궁", "진자로", "지로영", "엄핸림", "채즙모",
        "원노트", "천일염", "방소발", "공아밍", "염변국", "변설석", "양피지", "라지오",
        "견문석", "표암상", "반모혁", "맹국엄", "제기차", "계주인", "사이킥", "두반장",
        "설국차", "김축구", "채혈윤", "탁구재", "어명인", "범재율", "여객항", "호빅",
        "피자연", "가갸일", "목전진", "음박페", "석포터", "서이추", "운석현", "후아윤",
        "빈선덕", "권궐련", "필립", "돈각수", "탄착점", "삼벽조", "뇌이진", "이리듬",
        "비편문", "금억수", "위성락", "모나덕"
    };

    public GameObject prefab; // 생성할 Prefab
    public Transform contentTransform; // ScrollView의 Content를 연결

    private void Start()
    {
        GeneratePrefabs();
    }

    void GeneratePrefabs()
    {
        if (prefab == null || contentTransform == null)
        {
            Debug.LogError("프리팹 혹은 contentTransform이 없습니다.");
            return;
        }

        // 이름 리스트 랜덤 섞기
        ShuffleList(randomNameList);

        for (int i = 0; i < staticNameList.Count; i++) 
        {
            GameObject newPrefab = Instantiate(prefab, contentTransform); // Content에 추가
            newPrefab.name = staticNameList[i]; // staticNameList의 이름 사용

            // "Name"이라는 TextMeshProUGUI 컴포넌트에 이름 표시
            var nameText = newPrefab.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = staticNameList[i];
            }
            else
            {
                Debug.LogError($"{newPrefab.name} 프리팹에 TMP가 없습니다.");
            }

            var profile = newPrefab.transform.Find("Profile")?.GetComponent<Image>();
            if (profile != null)
            {
                profile.sprite = profileImages[i];
            }
        }

        // 이름 리스트에 따라 Prefab 생성
        foreach (var name in randomNameList)
        {
            GameObject newPrefab = Instantiate(prefab, contentTransform); // Content에 추가
            newPrefab.name = name;

            // "Name"이라는 TextMeshProUGUI 컴포넌트에 이름 표시
            var nameText = newPrefab.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = name;
            }
            else
            {
                Debug.LogError($"{newPrefab.name} 프리팹에 TMP가 없습니다.");
            }
        }
    }

    void ShuffleList(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
