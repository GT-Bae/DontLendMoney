using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameEvent : MonoBehaviour
{
    public GameObject OpeningUI;
    public GameObject jobComplete; // アルバイト完了時に生成
    public GameObject jobLowHealth; // アルバイトの体力不足時に生成
    public TMP_Text healthText; // 体力UI
    public TMP_Text dayText; // 日付UI
    public TMP_Text mymoneytext; // 所持金UI
    public TMP_Text recoveryText;  // 回収金UI
    public TMP_Text friendText; // 友達数UI
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    /*** SE
    0: お金の音
    1: 警告の音
    2: 睡眠の音
    3: ベッドの音
    4: エンディング
    ***/

    public Transform verticalView;
    public GameObject repayEventPrefab; // 回収金イベント用

    private List<(string name, int loanAmount, int remainDate)> loanList = new List<(string, int, int)>();
    public AudioSource specificAudioSource;
    public List<TMP_Text> interest;
    private int maxHealth = 3;
    private int maxFriends = 0;

    private ArbeitPositioner arbeitPositioner; // アルバイト配置コンポーネント
    private BedtoSleep bedtoSleep; // 睡眠のコンポーネント
    private GotoEnding gotoEnding; // エンディングのコンポーネント
    private CalendarManager calendarManager; // カレンダー管理のコンポーネント
    private ArticleManager articleManager; // 記事管理のコンポーネント
    private NameListPositioner nameListPositioner; // チャットリスト管理のコンポーネント
    private void Start()
    {
        arbeitPositioner = GetComponent<ArbeitPositioner>();
        bedtoSleep = GetComponent<BedtoSleep>();
        gotoEnding = GetComponent<GotoEnding>();
        calendarManager = GetComponent<CalendarManager>();
        articleManager = GetComponent<ArticleManager>();
        nameListPositioner = GetComponent<NameListPositioner>();
        arbeitPositioner.DailyArbeitPositioner(); // アルバイトをランダムに配置

        UpdateAllText();
    }

    public void UpdateAllText() // 全てのUI更新
    {
        maxHealth = PlayerPrefs.GetInt("MaxHealth");
        UpdateDayText();
        UpdateHealthText(PlayerPrefs.GetInt("CurrentHealth", maxHealth));
        UpdateMoneyText(PlayerPrefs.GetInt("MyMoney", 0));
        UpdateRecoveryText(PlayerPrefs.GetInt("Recovery", 0));
        UpdateFriendText(PlayerPrefs.GetInt("CurrentFriends", 0));
    }

    private void SaveGameData()// データをセーブ
    {
        PlayerPrefs.Save(); 
        Debug.Log("ゲームのデータがセーブされました。");
    }

    /*** ---------------テスト用----------------- ***/
    public void TestHealthIncrease() {
        PlayerPrefs.SetInt("CurrentHealth", maxHealth);
        UpdateHealthText(maxHealth);
    }

    /*** アルバイト ***/
    public void PerformAlba()
    {
        int currentHealth = PlayerPrefs.GetInt("CurrentHealth", maxHealth);
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        int payValue = PlayerPrefs.GetInt("PayValue", 0);
        int healthLossValue = PlayerPrefs.GetInt("HealthLossValue", 0);
        if ((currentHealth - healthLossValue) >= 0)
        {
            currentHealth -= healthLossValue;
            currentMoney += payValue;

            PlayerPrefs.SetInt("CurrentHealth", currentHealth);
            PlayerPrefs.SetInt("MyMoney", currentMoney);
            SaveGameData();

            UpdateHealthText(currentHealth);
            UpdateMoneyText(currentMoney);
            Debug.Log("アルバイトを完了しました。体力-" + healthLossValue + ", お金＋" + payValue);

            PlayAudio(0);
            Instantiate(jobComplete);
        }
        else
        {
            Instantiate(jobLowHealth);
            Debug.Log("体力が尽きたため、アルバイトはできません。");
        }
    }

    /*** 睡眠 ***/
    public void PerformSleep()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        int currentMoney = PlayerPrefs.GetInt("MyMoney", 0);
        if (currentDay < 28)
        {
            switch (currentDay) { //カレンダーに応じた支出
                case 2:
                    currentMoney -= 10;
                    break;
                case 9:
                    currentMoney -= 10;
                    break;
                case 10:
                    currentMoney -= 30;
                    break;
                case 13:
                    currentMoney -= 30;
                    break;
                case 16:
                    currentMoney -= 10;
                    break;
                case 18:
                    currentMoney -= 50;
                    break;
                case 20:
                    currentMoney -= 30;
                    break;
                case 23:
                    currentMoney -= 10;
                    break;
                case 27:
                    currentMoney -= 30;
                    break;
            }

            int hasLoan = PlayerPrefs.GetInt("hasLoan",0); // 利子増加
            if (hasLoan == 1) {
                if (currentDay <= 21){
                    currentMoney -= 3;
                } else {
                    currentMoney -= 15;
                }                
            }

            PlayerPrefs.SetInt("MyMoney", currentMoney);
            UpdateMoneyText(currentMoney);

            currentDay++;
            PlayerPrefs.SetInt("CurrentDay", currentDay); // 日付セーブ
            PlayerPrefs.SetInt("CurrentHealth", maxHealth); // 最大体力に戻す

            SaveGameData();
            UpdateDayText();
            UpdateHealthText(maxHealth);
            arbeitPositioner.DailyArbeitPositioner();
            articleManager.SetRandomArticles();
            bedtoSleep.FadeOutWithMessage();
            nameListPositioner.GenerateRandomChats(3);

            PlayAudio(2);
            
            // カレンダー更新
            switch (currentDay) {
                case 8:
                    calendarManager.UpdateDates();
                    calendarManager.SetTodo(2,"生活費\n-30千円");
                    calendarManager.SetTodo(5,"悪魔\n-30千円");
                    break;
                case 15:
                    calendarManager.UpdateDates();
                    calendarManager.SetTodo(2,"");
                    calendarManager.SetTodo(3,"家賃\n-50千円");
                    break;
                case 22:
                    calendarManager.UpdateDates();
                    calendarManager.SetTodo(3,"");
                    interest[0].text = "一日利子 5%";
                    interest[1].text = "一日利子 5%";
                    break;
                case 28:
                    articleManager.SetSpecialArticle("最近、悪魔による被害を受けた方はご確認","こんにちは。悪魔被害の防止に力を入れております、天使ギルドです。\n近頃、悪魔による被害が増加する傾向にあります。\nどうか被害を受けている方は、下の通報ボタンを押して支援を要請してください。\n市民の皆さまの平穏な一日をお祈りしております。\n天使ギルドより");
                    break;
            }

            // ストーリー進行
            switch (currentDay) {
                case 1:
                    nameListPositioner.GeneratePrefab(1);
                    nameListPositioner.GeneratePrefab(0);
                    break;
                case 2:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 3:
                    nameListPositioner.GeneratePrefab(1);
                    break;
                case 4:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 5:
                    nameListPositioner.GeneratePrefab(0);
                    break;
                case 6:
                    nameListPositioner.GeneratePrefab(1);
                    break;
                case 7:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 8:
                    nameListPositioner.GeneratePrefab(0);
                    break;
                case 10:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 13:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 15:
                    nameListPositioner.GeneratePrefab(1);
                    nameListPositioner.GeneratePrefab(0);
                    break;
                case 16:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 19:
                    nameListPositioner.GeneratePrefab(2);
                    break;
                case 22:
                    nameListPositioner.GeneratePrefab(2);
                    nameListPositioner.GeneratePrefab(0);
                    break;
                case 28:
                    nameListPositioner.GeneratePrefab(0);
                    break;
            }
        }
        else // 最後の日ならエンディングへ
        {
            specificAudioSource.Stop();
            PlayAudio(4);
            gotoEnding.endSetting();
            Debug.Log("エンディングにいく");
        }
    }

    /*** 回収金 ***/
    public void ReceiveRecovery(int returnAmount) {
        int currentRecovery = PlayerPrefs.GetInt("Recovery", 0);

        currentRecovery += returnAmount;
        PlayerPrefs.SetInt("Recovery", currentRecovery);
        SaveGameData();

        UpdateRecoveryText(currentRecovery);

        Debug.Log("回収金が更新されました: " + currentRecovery);
    }

    /*** 友達 ***/
    public void AddFriend()
    {
        int currentFriends = PlayerPrefs.GetInt("CurrentFriends", 0);

        if (currentFriends < maxFriends)
        {
            currentFriends++;
            PlayerPrefs.SetInt("CurrentFriends", currentFriends);
            SaveGameData();

            UpdateFriendText(currentFriends);
            Debug.Log("友達が追加されました。");
        }
        else
        {
            Debug.Log("最大友達数を超過できません。");
        }
    }

    /*** UI ***/
    private void UpdateHealthText(int health)
    {
        healthText.text = "体力：" + health + "/" + maxHealth;
    }

    private void UpdateDayText()
    {
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        dayText.text = currentDay + "日";
    }

    private void UpdateMoneyText(int money)
    {
        mymoneytext.text = "残高：" + money + "千円";
    }

    private void UpdateRecoveryText(int money)
    {
        recoveryText.text = "回収金：" + money + "千円";
    }

    private void UpdateFriendText(int friends)
    {
        maxFriends = PlayerPrefs.GetInt("MaxFriends",0);
        friendText.text = "友達：" + friends + "/" + maxFriends;
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void OpeningUIActive() {
        OpeningUI.SetActive(true);
    }

    public void PlayAudio(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
            Debug.Log("SEが再生しました: " + audioClips[index].name);
        }
        else
        {
            Debug.LogError("インデックス範囲ではありません: " + index);
        }
    }

    /*** 返済イベント ***/
    public void AddToList(string name, int loanAmount, int remainDate)
    {
        loanList.Add((name, loanAmount, remainDate));
        Debug.Log($"リストに追加: {name}, {loanAmount}, {remainDate}");
    }

    public void DecreaseDaysAndCheckEvents()
    {
        for (int i = 0; i < loanList.Count; i++)
        {
            var item = loanList[i];
            item.remainDate--;

            if (item.remainDate <= 0)
            {
                FriendsLoanEvent(item);
                loanList.RemoveAt(i);
                i--;
            }
            else
            {
                loanList[i] = item;
            }
        }
    }

    private void FriendsLoanEvent((string name, int loanValue, int remainDay) item)
    {
        float randomValue = Random.value;
        if (randomValue < 0.7f) { // 返済
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            PlayerPrefs.SetInt("MyMoney",currentMoney+item.loanValue);
            ReceiveRecovery(item.loanValue);
            UpdateAllText();
            spawnReturnPrefab(item.name,"は",item.loanValue,"千円を返しました。");
            Debug.Log($"{item.name}は返しました。");
        } else if (Random.value < 0.5f) { // 利子
            int currentMoney = PlayerPrefs.GetInt("MyMoney",0);
            item.loanValue *= 125;
            item.loanValue /= 100;
            PlayerPrefs.SetInt("MyMoney",currentMoney+item.loanValue);
            ReceiveRecovery(item.loanValue);
            UpdateAllText();
            spawnReturnPrefab(item.name,"は利子を含めて",item.loanValue,"千円を返しました。");
            Debug.Log($"{item.name}は利子もくれました.");
        } else { // 잠적
            spawnReturnPrefab(item.name,"は",item.loanValue,"千円を持って姿を消しました。");
            Debug.Log($"{item.name}は音信不通になりました。");
        }
    }

    private void spawnReturnPrefab(string str1, string str2, int money, string str3) {
        GameObject newPrefab = Instantiate(repayEventPrefab, verticalView);
        TMP_Text returnText = newPrefab.transform.Find("ReturnText").GetComponent<TMP_Text>();
        returnText.text = $"{str1} {str2} {money} {str3}";
    }
}


