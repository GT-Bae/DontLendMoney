using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class GotoEnding : MonoBehaviour
{
    public List<TMP_Text> title;
    public List<TMP_Text> content;
    public GameObject endingUI;

    public void endSetting() {
        int friends = PlayerPrefs.GetInt("CurrentFriends", 0);
        int bonus = PlayerPrefs.GetInt("Recovery", 0);
        int money = PlayerPrefs.GetInt("MyMoney", 0);
        money += bonus * 3;
        
        int hadloan = PlayerPrefs.GetInt("Loan",0);
        int hadReport = PlayerPrefs.GetInt("Report",0);
        
        // 친구 분기
        if (friends < 5) {
            title[0].text = "외딴 섬";
            content[0].text = "너무 앞만 보고 달려오면서 바쁘게만 살아왔던 탓일까요?\n얼굴을 맞대면서 웃을 수 있는 친구가 많이 없어졌습니다.\n어차피 바빠서 만날 시간도 없으니 괜찮다고 생각합니다.";
        } else if (friends < 10) {
            title[0].text = "친구여";
            content[0].text = "친구와 만날때면 언제나 편안합니다.\n학창시절 친구들과도 어느정도 연락합니다.\n평화로운 삶에 만족하며 지냅니다.";
        } else {
            title[0].text = "최고의 의리";
            content[0].text = "주변 사람은 모두 좋은 사람이었습니다.\n당신은 결과에 놀라면서도 한편으로는 만족했습니다.\n‘야 내가 고기 쏜다. 거절할시 딱밤’";
        }

        // 돈 분기
        if (money < 300) {
            title[1].text = "배고파";
            content[1].text = "돈이 없어서 급한대로 아르바이트를 뜁니다.\n돈이 부족해지니 마음도 뭔가 여유롭지 않습니다.\n이런 일을 만든 악마는 이미 갔으니 어찌할 방도가 없습니다.\n빨리 이 상황을 벗어나야만 하겠지요.";
        } else if (money < 1500) {
            title[1].text = "본전";
            content[1].text = "원금은 회수 하였습니다. 천만다행입니다.\n일이 이상하게 흘러갔더라면 다 잃었을지도 모르죠.\n이제 소동은 끝났으니 하고있던 일이나 계속 해야겠네요.";
        } else {
            title[1].text = "내기 성공";
            content[1].text = "당신의 엄청난 지략으로 악마의 돈을 많이 뜯는데 성공하였습니다.\n꽁돈이 많이 생겨서 기분이 좋습니다.\n악마는 울면서 돌아갔습니다.";
        }

        // 대출 분기
        switch (hadloan) {
            case 0:
                title[2].text = "안전한 자산";
                content[2].text = "변수가 될 일은 최대한 줄이는게 좋죠.\n당신은 대출을 거들떠도 보지 않았습니다.\n이자도 없고 빚도없는 활기찬 일상을 보냅니다.";
                break;
            case 1:
                title[2].text = "급전이 필요했다";
                content[2].text = "대출을 받았습니다.\n돈이 자꾸 빠져나가는데 어쩔 수 없었죠.\n이번 주 이자는 냈으니 한 시름 놓았네요.";
                break;
        }

        // 신고 분기
        switch (hadReport) {
            case 0:
                title[3].text = "쟤는 언제 갚으려나...";
                content[3].text = "28일을 버티긴 했지만 아직 받지 못한 돈이 많이 있습니다.\n좀 한가해질 때마다 친구에게 빌려준 돈이 생각납니다.\n애초에 빌려주지 말걸.";
                break;
            case 1:
                title[3].text = "친구관계에 돈 없다.";
                content[3].text = "‘야~ 친구니까 빌려줘라’ 라는 말은 허황된 말이라는 것을 깨달았습니다.\n오직 돈 만으로는 친구관계를 만들 수 없습니다.\n친구라는 관계에서 채무자와 채권자와의 관계로 변질되는 것은 한 순간입니다.";
                break;
        }

        endingUI.SetActive(true);
    }
}
