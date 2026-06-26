/*
 * 3つの記事をランダムに選択して表示し、呼び出しに応じて特別な記事を表示するクラス
 */

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ArticleManager : MonoBehaviour
{
    private List<string> articles = new List<string>
    {
        "「ミーム」とは何か？|ミーム（Meme）とは何か？\n「M」が２つ、「e」が２つで構成されている。\n似た発音の言葉としては\n「マミムメモ」がある。",
        "マーラータンの魅力|マーラータンの魅力を知らない人たちに言いたい。\n私もマーラータンを食べてみたいです。\n誰か私にも一杯おごってください。",
        "チョコミント新メニュー|先日、チョコミン党が\n新しいチョコミントメニューを開発した。\nチョコミン党代表のキム・ミンチョ氏は\n「満足のいく仕上がりだ」と満面の笑みを浮かべた。",
        "虫（バグ）退治に最高の裏技|デバッグをして祈祷を捧げれば解決です…絶対バグるな、頼むから",
        "背筋ピザの新メニューがバズる…|新メニュー『シワ伸ばしピザ』がなんと1万円で登場！！明日発売なので、ぜひ皆さん食べに来てください。背筋ピザの店長「背筋」より。（堂々）"
    };

    public TMP_Text[] articleTitles;
    public GameObject articleCanvasPrefab;

    private List<(string title, string content)> articleList = new List<(string, string)>();

    void Start()
    {
        InitializeArticles();
        SetRandomArticles();
    }

    // articleListを初期設定
    public void InitializeArticles()
    {
        foreach (string article in articles)
        {
            string[] parts = article.Split('|');
            articleList.Add((parts[0].Trim(), parts[1].Trim()));
        }
    }

    public void SetRandomArticles()
    {
        System.Random random = new System.Random();
        var selectedArticles = articleList.OrderBy(x => random.Next()).Take(3).ToList();

        // 記事の題名とクリックイベントを設定
        for (int i = 0; i < articleTitles.Length; i++)
        {
            articleTitles[i].text = selectedArticles[i].title;
            int index = i;
            articleTitles[i].GetComponent<Button>().onClick.RemoveAllListeners();
            articleTitles[i].GetComponent<Button>().onClick.AddListener(() => 
                ShowArticleCanvas(selectedArticles[index].title, selectedArticles[index].content, false));
        }
    }

    // 特別な記事を1番目に設定
    public void SetSpecialArticle(string title, string content)
    {
        articleTitles[0].text = title;
        articleTitles[0].GetComponent<Button>().onClick.RemoveAllListeners();
        articleTitles[0].GetComponent<Button>().onClick.AddListener(() => 
            ShowArticleCanvas(title, content, true));
    }

    // 記事を表示
    public void ShowArticleCanvas(string title, string content, bool isSpecial = false)
    {
        GameObject newCanvas = Instantiate(articleCanvasPrefab);
        TMP_Text[] textComponents = newCanvas.GetComponentsInChildren<TMP_Text>();
        textComponents[0].text = title;
        textComponents[1].text = content;

        if (isSpecial)
        {
            Transform reportTransform = newCanvas.transform.Find("Report");
            if (reportTransform != null)
            {
                reportTransform.gameObject.SetActive(true);
            }
        }
    }
}
