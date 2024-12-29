using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ArticleManager : MonoBehaviour
{
    private List<string> articles = new List<string>
    {
        "세계를 놀라게 한 남자|원래는 두개를 내려고 했으나\n세개를 낸 남자는 세계를 놀라게 했다.",
        "내 귀에 도청이 있어… 귀 평수 화제|최근 뉴스에 난입하여\n‘내 귀에 도청이 있어’\n라고 한 남자가 주목을 받고 있다.\n도대체 얼마나 넓길래 도청이 있을 수 있을까?\n\n후속 기사 원해요 213",
        "밈이란 무엇인가?|밈이란 무엇인가?\n‘ㅁ’ 2개와 ‘ㅣ’ 하나로 이루어져 있다.\n비슷한 발음의 단어로는 ‘미음’이 있다.",
        "마라탕의 매력|마라탕의 매력을 모르는 사람들에게 말하고 싶다.\n마라탕 저도 먹어보고 싶습니다.\n저도 하나 사주세요.",
        "민트초코 신메뉴|최근 민트미식회에서\n새로운 민트초코 메뉴를 개발하였다.\n민트미식회 회장 김민초씨는\n만족스러운 결과물이라며 입을 활짝 웃어보였다."
    };

    public TMP_Text[] articleTitles; // Article1~3의 TMP_Text 배열
    public GameObject articleCanvasPrefab; // 새로운 캔버스 프리팹

    private List<(string title, string content)> articleList = new List<(string, string)>();

    void Start()
    {
        InitializeArticles();
        SetRandomArticles();
    }

    void InitializeArticles()
    {
        foreach (string article in articles)
        {
            string[] parts = article.Split('|');
            articleList.Add((parts[0].Trim(), parts[1].Trim()));
        }
    }

    public void SetRandomArticles()
{
    // 랜덤으로 기사 선택
    System.Random random = new System.Random();
    var selectedArticles = articleList.OrderBy(x => random.Next()).Take(3).ToList();

    // 기사 제목 설정 및 버튼 클릭 이벤트 추가
    for (int i = 0; i < articleTitles.Length; i++)
    {
        articleTitles[i].text = selectedArticles[i].title;
        int index = i; // 로컬 변수로 인덱스 저장
        articleTitles[i].GetComponent<Button>().onClick.RemoveAllListeners(); // 기존 이벤트 제거
        articleTitles[i].GetComponent<Button>().onClick.AddListener(() => ShowArticleCanvas(selectedArticles[index]));
    }
}


    void ShowArticleCanvas((string title, string content) article)
    {
        GameObject newCanvas = Instantiate(articleCanvasPrefab);
        TMP_Text[] textComponents = newCanvas.GetComponentsInChildren<TMP_Text>();
        textComponents[0].text = article.title;
        textComponents[1].text = article.content;
    }

    public void SetSpecialArticle(string title, string content) // 첫 기사 변경
    {
        articleTitles[0].text = title;
        articleTitles[0].GetComponent<Button>().onClick.RemoveAllListeners();
        articleTitles[0].GetComponent<Button>().onClick.AddListener(() => ShowSpecialArticleCanvas(title, content));
    }

    void ShowSpecialArticleCanvas(string title, string content)
    {
        GameObject newCanvas = Instantiate(articleCanvasPrefab);
        TMP_Text[] textComponents = newCanvas.GetComponentsInChildren<TMP_Text>();
        textComponents[0].text = title;
        textComponents[1].text = content;

        // Report 오브젝트 활성화
        Transform reportTransform = newCanvas.transform.Find("Report");
        if (reportTransform != null)
        {
            reportTransform.gameObject.SetActive(true);
        }
    }
}