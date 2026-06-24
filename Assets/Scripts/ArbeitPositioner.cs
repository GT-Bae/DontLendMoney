/*
 * 3つのアルバイトをランダムに選択してJobInfoPrefabに表示するクラス
 */

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ArbeitPositioner : MonoBehaviour
{
    public GameObject jobInfoPrefab;
    public Transform parentTransform;

    private List<string> jobs = new List<string>
    {
        "バナナクリック, Banana",
        "虫捕まえてててっ, まじで虫無理",
        "バグ修正の祈祷, コード無事祈願協会",
        "チョコミント試食, チョコミン党",
        "迅速安全配達, 背筋ピザ"
    };

    private List<GameObject> spawnedJobInfos = new List<GameObject>();

    public void DailyArbeitPositioner()
    {
        List<string> selectedJobs = jobs.OrderBy(x => Random.value).Take(3).ToList();
        List<(string title, string recruiter)> jobList = new List<(string, string)>();
        foreach (string job in selectedJobs)
        {
            string[] parts = job.Split(',');
            jobList.Add((parts[0].Trim(), parts[1].Trim()));
        }

        foreach (GameObject jobInfo in spawnedJobInfos)
        {
            Destroy(jobInfo);
        }
        spawnedJobInfos.Clear();

        // JobInfoPrefabを召喚し設定
        for (int i = 0; i < jobList.Count; i++)
        {
            GameObject newJobInfo = Instantiate(jobInfoPrefab, parentTransform);
            spawnedJobInfos.Add(newJobInfo);

            TMP_Text titleText = newJobInfo.transform.Find("Title").GetComponent<TMP_Text>();
            TMP_Text recruiterText = newJobInfo.transform.Find("Recruiter").GetComponent<TMP_Text>();
            TMP_Text healthLossText = newJobInfo.transform.Find("HealthLoss").GetComponent<TMP_Text>();
            TMP_Text payText = newJobInfo.transform.Find("Pay").GetComponent<TMP_Text>();

            titleText.text = jobList[i].title;
            recruiterText.text = jobList[i].recruiter;

            if (i == 0)
            {
                healthLossText.text = "体力1 消耗";
                payText.text = Random.Range(3, 6).ToString() + "千円";
            }
            else if (i == 1)
            {
                healthLossText.text = "体力2 消耗";
                payText.text = Random.Range(9, 12).ToString() + "千円";
            }
            else if (i == 2)
            {
                healthLossText.text = "体力3 消耗";
                payText.text = Random.Range(17, 21).ToString() + "千円";
            }
        }
    }
}