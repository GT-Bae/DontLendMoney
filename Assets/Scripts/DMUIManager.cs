/*
 * 対話の選択肢と台詞を出力し、所持金を増減させるクラス
 */


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DMUIManager : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject optionsUIPrefab;
    public Transform content;
    public GameObject otherChatPrefab;
    public GameObject myChatPrefab;
    public float optionsHeight = 300f; // 選択肢UIのたかさ
    public GameObject warningLowMoney;
    private GameObject optionsUI = null;
    private RectTransform scrollViewRect;
    private RectTransform contentRect;
    private ScrollRect scrollRect;
    private bool optionsActive = false;
    private GameObject gameEventObject;
    public TMP_Text chatNameText;
    public List<Sprite> profiles;
    private bool hasChat = false;
    private bool choiceMade = false;
    private List<string> FriendBorrowMents = new List<string> {"急な事情があってさ","面目ないんだけど","悪いんだけど","久しぶりに連絡してこんなこと言うのも悪いんだけど","こんなことを言うのは恥ずかしいんだけど","こんなことしちゃいけないのは分かってるんだけど","本当にごめん","元気にしてる？","最近ちょっと大変でさ","無理なお願いで悪いんだけど","状況が良くなくてさ","こんなお願いをすることになるとは思わなかったんだけど","助けてもらえるとありがたい。ごめん。","迷惑なお願いだとは思うんだけど","どうしようもない状況でさ","助けてくれたら本当に忘れないよ","もし少しだけ助けてもらえないかな？","ちょっとお願いしてもいい？","こんなお願いしてごめん","悪いお願いなんだけど","連絡もしなかったのにお願いしてごめん","面目ないお願いなんだけど","藁にもすがる思いでお願いするよ","他に方法がなくてさ","忙しいだろうに本当にごめん","忙しい？お願いが一つあるんだけど","元気だよね？どうしようもない事情があってさ","一度だけ貸してくれないかな？","お金が足りなくてさ","どうしようもなくてお願いしてるんだけど","彼女にプレゼントをあげなきゃいけなくてさ","生活費が必要でさ","病院代が必要でさ","介護費が必要でさ","酒を飲まなきゃいけなくてさ","カード代を払わなきゃいけなくてさ","株をすぐに引き出せなくてさ","友達と旅行に行かなきゃいけなくてさ","お金が少し足りなくてさ","携帯代を早く払わなきゃいけなくてさ","食費が足りなくてさ","急にお金が必要でさ","ローンが組めなくてさ","投資に失敗してお金がなくてさ","ローンを返さなきゃいけなくてさ","急に集まりができてさ","急に大変なことが起きてさ","予想もしなかったのにクビになってさ","バイトをしてもお金が足りなくてさ","急に家に弔事ができてさ","大事な約束があってさ"};
    private List<int> borrowAmount = new List<int> {10,15,20,25,30,35,40,45,50};
    private List<string> FriendAgreeMents = new List<string> {"やっぱりお前は私の友達だ、ありがとう","本当にありがとう","ありがとう T_T","友よ、ありがとう","愛してるぞ、友よ！！","ありがとう、友よ","友よ、この恩は忘れない","必ず返すよ、友よ","友よ…つらい決断だっただろうに、ありがとう","急なお願いだったのにありがとう、友よ！！","友よ、この恩は忘れないよ","信じてくれてありがとう、友よ","友よ、絶対に返すから！！","忙しいだろうにありがとう","おかげで元気が出たよ、友よ","友よ、おかげで助かった","助けてくれて本当にありがとう、友よ","友よ、おかげで解決したよ！必ず返すね","ありがとう、必ず返すよ","友よ、お前がいて本当によかった","必ず返すよ","覚えておくよ","ちゃんと返すよ","つらい決断だっただろうにありがとう、友よ","大変なことになるところだった、ありがとう"};
    private List<string> FriendDenyMents = new List<string> {"お前、それでも友達か？","がっかりだよ、友よ","友達が苦しんでるのに助けてもくれないんだな","友よ、そんなことするのか？","私たちの友情ってたったこれだけだったのか？","心から失望したよ","私のこと無視してるのか？","私がつらいって言ってるのに！！！","友よ、私が何か悪いことしたか？","これはちょっとひどくないか？","こんなふうにされたら、頼んだ私はどんな気持ちになると思う？","それだけか？","お前、私と本当に友達か？","友達が助けてくれって言ってるのに、これでいいのか？","友達を助けるのが嫌なのか？","そうか","仕方ないよな","お前にも事情があるだろうし、分かるよ","分かった","ごめん","じゃあ用事済ませて","忙しいだろうにごめん","もう無視してくれ、私が考えてもこれは違うよな","ごめん、友達に借りるのはやっぱり違う気がする","邪魔してごめん","私が焦りすぎてた、ごめん","気を遣わせてごめん","無茶なお願いだったよ、ごめん","じゃあ今度ご飯でも一回食べよう","じゃあ元気で"};

    void Awake()
    {
        scrollViewRect = scrollView.GetComponent<RectTransform>();
        contentRect = content.GetComponent<RectTransform>();
        scrollRect = scrollView.GetComponent<ScrollRect>();
    }

    public void SetChatName(string name)
    {
        chatNameText.text = name;

        if (name != "悪魔" && name != "ジュン" && name != "ゴ") {
            AddOtherChat();
        } else if (name == "悪魔") {
            DemonChat();
        } else if (name == "ジュン") {
            BFFChat();
        } else if (name == "ゴ") {
            VillainChat();
        }
    }

    //選択肢の表示有無に応じてUIの位置調整
    public void ToggleOptionsUI()
    {
        ScrollToBottom();
        optionsActive = !optionsActive;
        
        if (optionsActive)
        {
            optionsUI = Instantiate(optionsUIPrefab);
            // ScrollViewの高さ縮小および位置調整
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y - optionsHeight);
            scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y + optionsHeight / 2);

            // Contentの位置調整
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y + optionsHeight);
        }
        else
        {
            // ScrollView高さ復元および位置を調整
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, scrollViewRect.sizeDelta.y + optionsHeight);
            scrollViewRect.anchoredPosition = new Vector2(scrollViewRect.anchoredPosition.x, scrollViewRect.anchoredPosition.y - optionsHeight / 2);

            // Contentの位置を元に戻す
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, contentRect.anchoredPosition.y - optionsHeight);

            Destroy(optionsUI);
        }
        Canvas.ForceUpdateCanvases();
    }

    //相手のメッセージを追加
    public void AddMessage(string message)
    {
        GameObject newChatBox = Instantiate(otherChatPrefab, content);
        TMP_Text[] textComponents = newChatBox.GetComponentsInChildren<TMP_Text>();
        Image[] profile = newChatBox.GetComponentsInChildren<Image>();

        TMP_Text nameText = null;
        TMP_Text messageText = null;

        foreach (TMP_Text textComponent in textComponents)
        {
            if (textComponent.name == "Name")
            {
                nameText = textComponent;
            }
            else if (textComponent.name == "Content")
            {
                messageText = textComponent;
            }
        }

        nameText.text = chatNameText.text;
        messageText.text = message;

        foreach (Image img in profile)
        {
            if (img.name == "Profile")
            {
                if (nameText.text == "悪魔") {
                    img.sprite = profiles[0];
                } else if (nameText.text == "ジュン") {
                    img.sprite = profiles[1];
                } else if (nameText.text == "ゴ") {
                    img.sprite = profiles[2];
                }
            }
        }

        Canvas.ForceUpdateCanvases();
        ScrollToBottom();
    }

    //プレイヤーのメッセージを追加
    public void AddMyMessage(string message)
    {
        GameObject newChatBox = Instantiate(myChatPrefab, content);
        TMP_Text messageText = newChatBox.GetComponentInChildren<TMP_Text>();
        messageText.text = message;

        Canvas.ForceUpdateCanvases();
        ScrollToBottom();
    }

    //最下部までスムーズにスクロール
    private IEnumerator SmoothScrollToBottom()
    {
        float scrollDuration = 0.5f;
        float elapsedTime = 0f;
        float startValue = scrollRect.verticalNormalizedPosition;
        float endValue = 0f;

        while (elapsedTime < scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startValue, endValue, elapsedTime / scrollDuration);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = endValue; // 最終位置を設定
    }

    //最下部へのスクロールを安全に実行するメソッド
    private void ScrollToBottom()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            StartCoroutine(SmoothScrollToBottom());
        }
        else
        {
            Debug.LogError("ScrollTectがインスペクターで割り当てられていません。");
        }
    }

    //毎日3人登場するランダムな友達（NPC）の生成
    private void AddOtherChat()
    {   
        if (hasChat == false) {
            int maxFriends = PlayerPrefs.GetInt("MaxFriends",0);
            PlayerPrefs.SetInt("MaxFriends",maxFriends+1);
            int numFriends = PlayerPrefs.GetInt("CurrentFriends", 0);
            PlayerPrefs.SetInt("CurrentFriends",numFriends+1);
            // ランダムに台詞および金額設定
            string randomMent = FriendBorrowMents[Random.Range(0, FriendBorrowMents.Count)];
            int randomAmount = borrowAmount[Random.Range(0, borrowAmount.Count)];
            int randomDate = Random.Range(1,4);

            string playerName = PlayerPrefs.GetString("Name", "ブランク");

            AddMessage($"{playerName}、{randomMent}\n{randomAmount}千円貸せる?");
            AddMessage($"{randomDate}日後に返すよ。");
            
            ToggleOptionsUI();

            Button button1 = optionsUI.transform.Find("Button1").GetComponent<Button>();
            Button button2 = optionsUI.transform.Find("Button2").GetComponent<Button>();
            button1.GetComponentInChildren<TMP_Text>().text = "貸す";
            button2.GetComponentInChildren<TMP_Text>().text = "貸さない";

            button1.onClick.AddListener(() => OnAgree(randomAmount,randomDate));
            button2.onClick.AddListener(() => OnDeny());
        } else {
            Debug.Log("すでにみた対話です。");
        }
    }

    //プレイヤーが「貸す」を選択した時の処理
    private void OnAgree(int randomAmount, int randomDate)
    {
        int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
        if (currentMoney < randomAmount) {
            Instantiate(warningLowMoney);
        } else {
            GameObject gameEventObject = GameObject.Find("GameManager");
            GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
            PlayerPrefs.SetInt("MyMoney",currentMoney-randomAmount);
            AddMyMessage("分かった");

            // ランダムに同意台詞出力
            string randomAgreeMent = FriendAgreeMents[Random.Range(0, FriendAgreeMents.Count)];
            AddMessage(randomAgreeMent);

            int health = PlayerPrefs.GetInt("CurrentHealth",0);
            PlayerPrefs.SetInt("CurrentHealth",health-1);

            gameEvent.AddToList(chatNameText.text, randomAmount, randomDate);
            gameEvent.UpdateAllText();

            ToggleOptionsUI();
            hasChatted();
        }
    }

    //プレイヤーが「貸さない」を選択した時の処理
    private void OnDeny()
    {
        AddMyMessage("無理");
        // ランダムに拒絶台詞出力
        string randomDenyMent = FriendDenyMents[Random.Range(0, FriendDenyMents.Count)];
        AddMessage(randomDenyMent);

        // CurrentFriendsを-1
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0);
        PlayerPrefs.SetInt("CurrentFriends", currentFriends - 1);
        
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        gameEvent.UpdateAllText();
        
        ToggleOptionsUI();
        hasChatted();
    }

    //チャットを見たことあるかのプラグ
    public void hasChatted() {
        hasChat = true;
    }

    public IEnumerator WaitForThreeSeconds()
    {
        yield return new WaitForSeconds(3f);
    }
    
    //選択肢が決定されるまでコルーチンを待機させる
    public IEnumerator WaitForChoice()
    {
        choiceMade = false;
        yield return new WaitUntil(() => choiceMade);
    }
    
    //選択肢UIのテキスト設定とイベント登録
    private void ToggleOptionsUIStory(string str1, string str2) {
        ToggleOptionsUI();
        Button button1 = optionsUI.transform.Find("Button1").GetComponent<Button>();
        Button button2 = optionsUI.transform.Find("Button2").GetComponent<Button>();
        button1.onClick.AddListener(() => buttonPrint(button1.GetComponentInChildren<TMP_Text>().text));
        button2.onClick.AddListener(() => buttonPrint(button2.GetComponentInChildren<TMP_Text>().text));
        button1.GetComponentInChildren<TMP_Text>().text = $"{str1}";
        button2.GetComponentInChildren<TMP_Text>().text = $"{str2}";
    }

    //選択肢が選ばれた際の処理
    private void buttonPrint(string content) {
        AddMyMessage(content);
        OnChoiceMade();
        ToggleOptionsUI();
    }

    private void OnChoiceMade()
    {
        choiceMade = true;
    }

    public void DemonChat() {
        StartCoroutine(DemonChatCoroutine());
    }

    public void BFFChat() {
        StartCoroutine(BFFChatCoroutine());
    }

    public void VillainChat() {
        StartCoroutine(VillainCahatCoroutine());
    }

    public IEnumerator DemonChatCoroutine() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay",0);
        string name = PlayerPrefs.GetString("Name","ブランク");
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        if (currentDay == 1 && hasChat == false) {
            AddMessage("私のこと知ってるよな？");
            ToggleOptionsUIStory("何だよ、なんで私の友達のことを気にしてるんだ？","お前誰だよ、いきなりタメ口か？");
            yield return WaitForChoice();          
            AddMessage("気にならないか？ 賭けを一つしよう");
            ToggleOptionsUIStory("別に気にはならないけど", "普通に生きてちゃだめなの？");
            yield return WaitForChoice();
            AddMessage("28日間、お前の友達から金を貸してくれってメッセージが届く。");
            yield return new WaitForSeconds(2f);
            AddMessage("そして最終日までに、できるだけ多くの金を取り返してみろ。");
            ToggleOptionsUIStory("やりたくないんだけど", "私が損するだけじゃない？");
            yield return WaitForChoice();
            AddMessage("回収した金の3倍をやる。どうだ？");
            ToggleOptionsUIStory("悪くないな？", "最悪なんだけど？");
            yield return WaitForChoice();
            AddMessage($"いいから！ 契約書でも書こう。名前は{name}で、期間は28日…");
            yield return new WaitForSeconds(2f);
            AddMessage("…よし…じゃあ28日間よろしくな！");
            ToggleOptionsUIStory("やだ", "契約破棄したい");
            yield return WaitForChoice();
            AddMessage("(オフライン状態です。- 連絡禁止)");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 5 && hasChat == false) {
            AddMessage("ゴフンがお金を貸してくれって言ったら、必ず貸してやれ。");
            ToggleOptionsUIStory("なんで？","嫌なんだけど？");
            yield return WaitForChoice();
            AddMessage("なぜかあいつ、気に入ったんだよ");
            yield return new WaitForSeconds(2f);
            AddMessage("貸さなかったら、そのままお前の金を持って帰るぞ");
            yield return new WaitForSeconds(2f);
            AddMessage("契約書第2条第3項に書いてある");
            ToggleOptionsUIStory("用意周到だな","契約書第6条第2項に、お前が私に殴られなきゃいけないって書いてないのか？");
            yield return WaitForChoice();
            AddMessage("(オフライン状態です。- 連絡禁止)");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 8 && hasChat == false) {
            AddMessage("このまま放っておくのもつまらないな");
            ToggleOptionsUIStory("何言ってんだ？", "今度はまた何するつもりだ？");
            yield return WaitForChoice();
            AddMessage("これから毎週金曜日ごとに、私に30千円ずつ献上しろ。");
            ToggleOptionsUIStory("ただのクズじゃないか？", "さすがにこれはないだろ");
            yield return WaitForChoice();
            AddMessage("私は給料ももらわずにお前をいじめてやってるのに、やってられなくてな。文句あるか？");
            ToggleOptionsUIStory("私の給料が減るじゃん", "じゃあ私は？");
            yield return WaitForChoice();
            AddMessage("(オフライン状態です。- 連絡禁止)");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 15 && hasChat == false) {
            int currentHealth = PlayerPrefs.GetInt("CurrentHealth", 0);
            int maxHealth = PlayerPrefs.GetInt("MaxHealth", 0);
            AddMessage("画像を送信しました。");
            AddMessage("動画を送信しました。");
            yield return new WaitForSeconds(2f);
            AddMessage("お金を送金しました。");
            ToggleOptionsUIStory("何してんだ？", "中指を送りました。");
            yield return WaitForChoice();
            AddMessage("最近、なんだか力が出なくてな");
            yield return new WaitForSeconds(2f);
            AddMessage("しばらくお前の体力を1つ没収だ。");
            ToggleOptionsUIStory("具合が悪いのはお前のせいなのに、なんで私に当たるんだ？", "私もお前の角を1本没収したい");
            yield return WaitForChoice();
            AddMessage("うるさい、風邪気味だから寝る。");
            ToggleOptionsUIStory("これでそのまま帰るのか？", "養生なんて知るか、凍えて死ね");
            yield return WaitForChoice();
            AddMessage("(オフライン状態です。- 連絡禁止)");
            PlayerPrefs.SetInt("CurrentHealth",currentHealth-1);
            PlayerPrefs.SetInt("MaxHealth",maxHealth-1);
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 22 && hasChat == false) {
            AddMessage("お前、ローンでも組んでるのか？");
            ToggleOptionsUIStory("あったらどうするんだよ？", "知る必要なくないか？");
            yield return WaitForChoice();
            int hasLoan = PlayerPrefs.GetInt("hasLoan",0);
            if (hasLoan == 0)
                AddMessage("大したことじゃない、やることでもやってろ。");
            else
                AddMessage("あるんだな？ 日利を5%に上げたから、きっちり返せよ。");
            ToggleOptionsUIStory("何だよ？", "おい");
            yield return WaitForChoice();
            AddMessage("(オフライン状態です。- 連絡禁止)");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 28 && hasChat == false) {
            AddMessage("もう最終日か。");
            ToggleOptionsUIStory("だから何だよ", "じゃあな");
            yield return WaitForChoice();
            AddMessage("やっとこの腐りきった家を出ていくのか。");
            ToggleOptionsUIStory("何だよ", "私の家にいたのか？");
            yield return WaitForChoice();
            AddMessage("一人暮らしだと寂しいだろうから、邪魔しに来たんだよ。");
            ToggleOptionsUIStory("掃除でもしろよ", "洗濯でもしろよ");
            yield return WaitForChoice();
            AddMessage("うるさい！ また明日な。");
            ToggleOptionsUIStory("何だ？", "おい、起きろよ");
            yield return WaitForChoice();
            AddMessage("(オフライン状態です。- 連絡禁止)");
            gameEvent.UpdateAllText();
            hasChatted();
        }
    }

    public IEnumerator BFFChatCoroutine() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay",0);
        string name = PlayerPrefs.GetString("Name","민수");
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        if (currentDay == 1 && hasChat == false) {
            AddMessage("よ、元気してる？");
            ToggleOptionsUIStory("うん", "お前よりは");
            yield return WaitForChoice();
            AddMessage("こいつ…生きてはいるんだなｗ");
            ToggleOptionsUIStory("そっくりそのまま返す", "死体がしゃべってるな");
            yield return WaitForChoice();
            AddMessage("死にたいのか？！");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 3 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage($"{name}");
            ToggleOptionsUIStory("何？", "どうした？");
            yield return WaitForChoice();
            AddMessage("20千円だけ貸して。3日後には返せる。");
            ToggleOptionsUIStory("おっけー", "返さなかったら殺すぞ");
            yield return WaitForChoice();
            AddMessage("返さなかったらお前の前で醤油シャワーするわ");
            PlayerPrefs.SetInt("MyMoney",currentMoney-20);
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 6 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage("ありがたく使ったよん、サンキューサンキュー！");
            AddMessage("[25千円送金]");
            ToggleOptionsUIStory("ナイス", "よかったな");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney+25);
            int recovery = PlayerPrefs.GetInt("Recovery",0);
            PlayerPrefs.SetInt("Recovery",recovery+25);
            AddMessage("おつかれ！");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 15 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage("誕おめｗ");
            AddMessage("[50千円送金]");
            ToggleOptionsUIStory("何これ？", "何か変なものでも食べた？");
            yield return WaitForChoice();
            AddMessage("お前も私の誕生日のとき送ってくれただろ。");
            yield return new WaitForSeconds(2f);
            AddMessage("期待してるね～～");
            ToggleOptionsUIStory("終わったな", "ありがと");
            yield return WaitForChoice();
            AddMessage("はいはい、そのへんで。");
            PlayerPrefs.SetInt("MyMoney",currentMoney+50);
            gameEvent.UpdateAllText();
            hasChatted();
        }
    }


    public IEnumerator VillainCahatCoroutine() {
        int currentDay = PlayerPrefs.GetInt("CurrentDay",0);
        string name = PlayerPrefs.GetString("Name","민수");
        GameObject gameEventObject = GameObject.Find("GameManager");
        GameEvent gameEvent = gameEventObject.GetComponent<GameEvent>();
        if (currentDay == 2 && hasChat == false) {
            AddMessage("おい、元気か？");
            ToggleOptionsUIStory("ああ、元気にしてる。", "お前は？");
            yield return WaitForChoice();
            AddMessage("それはよかった。");
            ToggleOptionsUIStory("急にどうした？", "?");
            yield return WaitForChoice();
            AddMessage("ただ安否確認してるだけだよ。");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 4 && hasChat == false) {
            AddMessage("変わりないよな？");
            ToggleOptionsUIStory("YouTubeでも見てるだけだよ。", "せいぜい見とけ。");
            yield return WaitForChoice();
            AddMessage("私はインスタを熱心に見てるところ。");
            ToggleOptionsUIStory("せいぜい見とけ。", "そうか。");
            yield return WaitForChoice();
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 7 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            AddMessage($"{name}、急なお願いで悪いんだけど、明日お金を使う用事ができたんだ。本当に二日後までには返せるから、ちょっと振り込んでもらえないかな？");
            ToggleOptionsUIStory("何があったんだ？", "急に？");
            yield return WaitForChoice();
            AddMessage("明日母さんの誕生日なんだけど、今まで一度もプレゼントしてなかったんだ。お金を移そうとしたら、限度額のせいで引っかかってしまって。");
            yield return new WaitForSeconds(2f);
            AddMessage("本当にお金は二日後に必ず返すよ。");
            ToggleOptionsUIStory("いくら必要なんだ？", "分かった。");
            yield return WaitForChoice();
            AddMessage("50千円あればいい。");
            PlayerPrefs.SetInt("MyMoney",currentMoney-50);
            yield return new WaitForSeconds(2f);
            AddMessage("あ、本当にありがとう二日後に必ず送るよ。");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 10 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            ToggleOptionsUIStory("昨日までに返すはずじゃなかったか？","もしもし？");
            yield return WaitForChoice();
            AddMessage($"{name}、私さ、首都圏に住んでないのに用事があってこっちに来たんだけど、お金がなくて家に帰れないんだ。");
            ToggleOptionsUIStory("何だって？", "今どきスマホで銀行アプリ使えるだろ。");
            yield return WaitForChoice();
            AddMessage("あ、本当にごめん。最近積立を解約したんだけど、それが入ってきたら返そうと思ってたんだ。");
            yield return new WaitForSeconds(2f);
            AddMessage("電話してみたら、入るのは二日後だって。");
            yield return new WaitForSeconds(2f);
            AddMessage("とりあえず寝なきゃいけなさそうなんだけど、宿泊費を出してくれないか？");
            ToggleOptionsUIStory("用事で首都圏に行ったのに金がないって？", "正気か？");
            yield return WaitForChoice();
            AddMessage("本当に悪い。今は本当に事情があるんだ。");
            yield return new WaitForSeconds(2f);
            AddMessage("宿泊費と交通費と食費を少し足してくれ。");
            ToggleOptionsUIStory("ふざけてるのか？", "いくら必要なんだ？");
            yield return WaitForChoice();
            AddMessage("宿泊費10＋交通費10＋食費1で、21千円あればいい。");
            ToggleOptionsUIStory("しっかりしろよ。", "まいったな。");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney-21);
            AddMessage("信じてくれてありがとう。本当に二日後に送るよ。");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 13 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            ToggleOptionsUIStory("狼少年だって嘘は二回までだったぞ。","メェヘヘヘ");
            yield return WaitForChoice();
            AddMessage("実は先月キャリア決済した分があって、それを返さなきゃいけないんだ。\n100千円だけ貸してもらえないかな？");
            ToggleOptionsUIStory("100千円って犬の名前かよ？", "ふざけてるのか？");
            yield return WaitForChoice();
            AddMessage("今回が本当に限界なんだ。返せなかったら大変なことになる。頼む。");
            ToggleOptionsUIStory("何したらそんな大変なことになるんだ？", "それで？");
            yield return WaitForChoice();
            AddMessage("正直に話すよ。");
            yield return new WaitForSeconds(4f);
            AddMessage("実は私、違法賭博をしてた。");
            yield return new WaitForSeconds(2f);
            AddMessage("これからは気を入れ直して、普通の生活を送ろうと思ってる。");
            ToggleOptionsUIStory("それで？", "だから何だよ？");
            yield return WaitForChoice();
            AddMessage("本当に100千円だけ貸してくれ。二日後の給料日に返すから。");
            ToggleOptionsUIStory("ギャンブルなんかするな。", "ほんと勘弁してくれ。");
            yield return WaitForChoice();
            AddMessage("ああ、もう本当に二度としない。ゴフン、頼むから目を覚ませ。");
            yield return new WaitForSeconds(2f);
            PlayerPrefs.SetInt("MyMoney",currentMoney-100);
            AddMessage("信じて貸してくれてありがとう 給料をもらったらすぐ返すよ");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 16 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            ToggleOptionsUIStory("給料はどこ行ったんだ？","これどういうシチュエーションなんだ？");
            yield return WaitForChoice();
            AddMessage("ああ、本当に悪い。友達が通報するって言うから、そいつに先に返したんだ。");
            ToggleOptionsUIStory("ふざけてるのか？", "どういうつもりだ？");
            yield return WaitForChoice();
            AddMessage("本当に、私は本気なんだ。");
            yield return new WaitForSeconds(2f);
            AddMessage("夜のバイトもしながら金を埋めてるところなんだ。");
            yield return new WaitForSeconds(2f);
            AddMessage("信じてくれ。金は必ず返す。");
            ToggleOptionsUIStory("ああ、絶対返せよ。", "約束は守らなきゃいけないって分かってるよな？");
            yield return WaitForChoice();
            AddMessage("それでなんだけど、家賃90千円滞納してて、貸してくれないか？");
            yield return new WaitForSeconds(2f);
            AddMessage("本当に切羽詰まってるんだ。");
            ToggleOptionsUIStory("金を返す気はあるのか？", "マジでふざけてるのか？");
            yield return WaitForChoice();
            AddMessage("本当に切実なんだ。バイトもしてるから、あまり長く話せない。");
            yield return new WaitForSeconds(2f);
            AddMessage("90千円だけ貸してくれ。本当に二日後に必ず返すから。");
            ToggleOptionsUIStory("必ず返せよ。", "私はなんでこんなことしてるんだろう");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney-90);
            AddMessage("本当に返せる。ありがとう。");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 19 && hasChat == false) {
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            int bigMoney = 0;
            ToggleOptionsUIStory("おい。","何してる？");
            yield return WaitForChoice();
            AddMessage("私、ギャンブルしてたじゃん。それで通報されて、まず罰金を払ったんだ。");
            ToggleOptionsUIStory("それで？", "なんでそんな話を私にするんだ？");
            yield return WaitForChoice();
            AddMessage("まだ罰金が残ってるんだけど、本当に最後に貸してもらえないか？");
            ToggleOptionsUIStory("頼むから、こういうこともうやめてくれないか？", "私は金を返してほしいんだよ。");
            yield return WaitForChoice();
            AddMessage("ああ、本当にこれが最後だ。誓うよ。");
            ToggleOptionsUIStory("お前の言葉が信じられると思うか？", "お前だったら貸すか？");
            yield return WaitForChoice();
            AddMessage("本当に最後に一回だけ借りる。");
            yield return new WaitForSeconds(2f);
            AddMessage("本当に本気なんだ。");
            yield return new WaitForSeconds(2f);
            if (currentMoney/2 >= 300) {
                bigMoney = 300;
            } else {
                bigMoney = currentMoney/2;
            }
            AddMessage($"{bigMoney}千円だ。");
            ToggleOptionsUIStory("私がバカだった。", "ああもう！！！");
            yield return WaitForChoice();
            PlayerPrefs.SetInt("MyMoney",currentMoney-bigMoney);
            AddMessage("ああ、本当に本当にありがとう。必ず二日後に返すよ(T_T)");
            gameEvent.UpdateAllText();
            hasChatted();
        } else if (currentDay == 22 && hasChat == false) {
            ToggleOptionsUIStory("早く返さないのか？","起きろ起きろ！！！");
            yield return WaitForChoice();
            AddMessage("ああ、もう本当にきつい。");
            yield return new WaitForSeconds(2f);
            AddMessage("こっちでも催促されて、あっちでも催促されて。");
            yield return new WaitForSeconds(2f);
            AddMessage("もう通報されて、そのままムショにでも行こうかな。");
            ToggleOptionsUIStory("おい。", "何だって？");
            yield return WaitForChoice();
            AddMessage("(未読のまま)");
            gameEvent.UpdateAllText();
            hasChatted();
        }
    }
}