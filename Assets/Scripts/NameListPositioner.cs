/*
 * チャット画面の名前リストを制御するクラス
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameListPositioner : MonoBehaviour
{
    public Sprite[] profileImages; // プロフィール画像配列
    private List<string> staticNameList = new List<string>
    {"悪魔", "ジュン", "ゴ"}; //主要キャラクター
    private List<string> randomNameList = new List<string>
    {"亜","哀","挨","愛","曖","悪","握","圧","扱","宛","金","安","案","暗","以","衣","位","囲","医","依","委","威","為","畏","胃","尉","異","移","萎","偉","椅","彙","意","違","維","慰","遺","緯","域","育","一","壱","逸","茨","芋","引","印","因","咽","姻","員","院","淫","陰","飲","隠","韻","右","宇","羽","雨","唄","鬱","畝","浦","運","雲","永","泳","英","映","栄","営","詠","影","鋭","衛","易","疫","益","液","駅","悦","越","謁","閲","円","延","沿","炎","怨","宴","媛","援","園","煙","猿","遠","鉛","塩"};

    public GameObject chatButtonPrefab;
    public Transform contentTransform;
    private void Start()
    {
        GenerateRandomChats(3);
    }

    public void GeneratePrefab(int i)
    {
        if (chatButtonPrefab == null || contentTransform == null)
        {
            Debug.LogError("chatButtonPrefabやcontentTransformが連結されていません");
            return;
        }

        ShuffleList(randomNameList);     
        GameObject newChatButton = Instantiate(chatButtonPrefab, contentTransform);

        var nameText = newChatButton.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null)
        {
            nameText.text = staticNameList[i];
        }
        else
        {
            Debug.LogError($"{newChatButton.name}  プレハブにTextMeshProUGUIコンポーネントがアタッチされていません。");
            throw new MissingComponentException();
        }

        var profile = newChatButton.transform.Find("Profile")?.GetComponent<Image>();
        if (profile != null)
        {
            profile.sprite = profileImages[i];
        }
    }

    public void GenerateRandomChats(int count)
    {
        //既存のオブジェクトを削除
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        List<string> selectedNames = new List<string>(randomNameList);
        ShuffleList(selectedNames);
        for (int i = 0; i < count; i++)
        {
            string selectedName = selectedNames[i];
            GameObject newPrefab = Instantiate(chatButtonPrefab, contentTransform);

            var nameText = newPrefab.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = selectedName;
            }
            else
            {
                Debug.LogError($"{newPrefab.name}  プレハブにTextMeshProUGUIコンポーネントがアタッチされていません。");
                throw new MissingComponentException();
            }

            //重複生成を防ぐため、選択された名前をリストから除外
            randomNameList.Remove(selectedName);
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