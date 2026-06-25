/*
 * 分岐に応じてエンディングの台詞を切り替えるクラス
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class GotoEnding : MonoBehaviour
{
    public List<Image> images;
    public List<Sprite> sprites;
    public List<TMP_Text> title;
    public List<TMP_Text> content;
    public GameObject endingUI;

    public void endSetting() {
        int friends = PlayerPrefs.GetInt("CurrentFriends", 0);
        int bonus = PlayerPrefs.GetInt("Recovery", 0); //友達からの返済金は3倍に計算
        int money = PlayerPrefs.GetInt("MyMoney", 0);
        money += bonus * 3;
        
        int hadloan = PlayerPrefs.GetInt("Loan",0);
        int hadReport = PlayerPrefs.GetInt("Report",0);
        
        // 友達分岐
        if (friends < 5) {
            images[0].sprite = sprites[0];
            title[0].text = "孤島";
            content[0].text = "前だけを見て走り続け、忙しく生きてきたせいでしょうか？\n顔を合わせて笑い合える友達が、ずいぶん少なくなってしまいました。\nどうせ忙しくて会う時間もないので、これでいいのだと思っています。";
        } else if (friends < 10) {
            images[0].sprite = sprites[1];
            title[0].text = "友よ";
            content[0].text = "友達と会うと、いつも気が楽になります。\n学生時代の友達とも、ある程度は連絡を取り合っています。\n穏やかな暮らしに満足して過ごしています。";
        } else {
            images[0].sprite = sprites[2];
            title[0].text = "最高の義理";
            content[0].text = "周りの人はみんな良い人でした。\nあなたはこの結果に驚きつつも、どこか満足していました。\n「今日はみんなで食い倒れよう！！」";
        }

        // お金分岐
        if (money < 500) {
            images[1].sprite = sprites[3];
            title[1].text = "腹ぺこ";
            content[1].text = "お金がないので、とりあえずアルバイトを入れます。\nお金が足りなくなると、心にもなんだか余裕がなくなります。\nこんな事態を招いた悪魔はもういなくなってしまい、どうしようもありません。\n早くこの状況から抜け出さなければなりませんね。";
        } else if (money < 1500) {
            images[1].sprite = sprites[4];
            title[1].text = "元は取れた";
            content[1].text = "ある程度のお金は守りきれました。\n少しでも流れが悪ければ、全部失っていたかもしれません。\nもう騒動は終わったので、やっていたことを続けるしかなさそうです。";
        } else {
            images[1].sprite = sprites[5];
            title[1].text = "賭けに勝利";
            content[1].text = "あなたの見事な策略で、悪魔から大金を巻き上げることに成功しました。\nお金がたくさん増えて、気分も上々です。\n悪魔は泣きながら帰っていきました。";
        }

        // ローン分岐
        switch (hadloan) {
            case 0:
                images[2].sprite = sprites[6];
                title[2].text = "安全な資産";
                content[2].text = "不確定要素は、できるだけ減らしたいものです。\nあなたはローンに見向きもしませんでした。\n利子も借金もない、活気ある日々を過ごしています。";
                break;
            case 1:
                images[2].sprite = sprites[7];
                title[2].text = "急な金が必要だった";
                content[2].text = "ローンを組みました。\nお金がどんどん減っていきますが、仕方ありませんでした。\n今月の利子は払えたので、ひとまず安心です。";
                break;
        }

        // 通報分岐
        switch (hadReport) {
            case 0:
                images[3].sprite = sprites[8];
                title[3].text = "あいつ、いつ返すんだろう...";
                content[3].text = "28日間はなんとか耐えましたが、まだ回収できていないお金がたくさんあります。\n少し暇になるたびに、友達に貸したお金のことを思い出します。\n最初から貸さなければよかった。";
                break;
            case 1:
                images[3].sprite = sprites[9];
                title[3].text = "友人関係に金はない。";
                content[3].text = "「おい〜、友達なんだから貸してくれよ」という言葉が、いかに都合のいいものかを思い知りました。\nお金だけで友人関係を築くことはできません。\n友達という関係が、債務者と債権者の関係に変わってしまうのは\n一瞬のことです。\n天使ギルドが、あなたを苦しめた悪魔を拘束しました。";
                break;
        }

        endingUI.SetActive(true);
    }
}
