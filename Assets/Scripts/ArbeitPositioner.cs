using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ArbeitPositioner : MonoBehaviour
{
    public GameObject jobInfoPrefab; // JobInfo 프리팹을 할당하세요
    public Transform parentTransform; // JobInfo 오브젝트를 배치할 부모 트랜스폼

    // 알바 목록
    private List<string> jobs = new List<string>
    {
        "돌하르방 알바,재즈도제주다",
        "진짜 ㄹㅇ 급구,MZebra",
        "미션흑기사,흑기사협회",
        "대타 모집,당근99도",
        "돌탑짓기 알바,돌탑수호대",
        "세차 알바,조수간만의차",
        "일일알바 모집,화투플레이스",
        "아무나 모집,크로와상하차",
        "버터정리 알바,버터스터미널",
        "당도최고 홍보,사바사과협회",
        "렌즈로불붙이기,렌즈탐구회",
        "화자심경탐구,화자맞아",
        "병풍알바 모집,프리킥협회",
        "친절한슈퍼맨,바나나돌가게",
        "은신닌자 알바,은신술센터",
        "야 급해불좀,꺼주고가라",
        "풍선 대타,안둥소주유소",
        "수석졸업모집,아주좋수석전문",
        "허수아비 알바,허수할아비",
        "선물포장 알바,산타할아버지",
        "천장매달리기,박쥐행동협회",
        "고래정비 알바,울산포경왕",
        "AI방역 알바,조류건강협회",
        "버그수정기도,코드무탈협회",
        "눈오리제작,청둥오리의꿈",
        "별 관찰 알바,우쥬라잌우주",
        "말동무 모집,진짜유니콘",
        "동무 모집,리평흠",
        "봄 수호 알바,웰던으로익힘",
        "빵하고뛰기,빵배달전문점",
        "신속안전배달,허리피자집",
        "눕방 스태프,NOOP",
        "가족.같은~알바.,가지옥물산",
        "과학실 해골 알바,센주",
        "방방관리 알바,방방봉봉덤블링",
        "요즘핫한아파트,시멘트중년단",
        "햄스터이사,이사팔삽십이",
        "민트초코 시식,민트미식회",
        "레시피 개발,우주괴식협회",
        "복순이홍보,복순이주인",
        "수련키우기,으뜸수련원",
        "창고정리,제3보급창고",
        "교단관리,Lamb교주",
        "얼차려대리 급구,당근0도",
        "벌레좀자ㅂㅏ즈ㅠ,아니벌레제발",
        "빙판아이스크림,5초이상폐기",
        "바나나클릭,Banana",
        "카운터 알바,고담시"
    };

    // 생성된 JobInfo 오브젝트를 저장할 리스트
    private List<GameObject> instantiatedJobInfos = new List<GameObject>();

    public void DailyArbeitPositioner()
    {
        // 제목과 모집사 세트를 랜덤으로 3개 뽑기
        List<string> selectedJobs = jobs.OrderBy(x => Random.value).Take(3).ToList();

        // 새로운 리스트에 추가
        List<(string title, string recruiter)> jobList = new List<(string, string)>();
        foreach (string job in selectedJobs)
        {
            string[] parts = job.Split(',');
            jobList.Add((parts[0].Trim(), parts[1].Trim()));
        }

        // 기존에 생성된 JobInfo 오브젝트 삭제
        foreach (GameObject jobInfo in instantiatedJobInfos)
        {
            Destroy(jobInfo);
        }
        instantiatedJobInfos.Clear();

        // JobInfo 프리팹 생성 및 설정
        for (int i = 0; i < jobList.Count; i++)
        {
            // JobInfo 프리팹 인스턴스화
            GameObject newJobInfo = Instantiate(jobInfoPrefab, parentTransform);
            instantiatedJobInfos.Add(newJobInfo);

            // JobInfo 오브젝트의 텍스트 컴포넌트 설정
            TMP_Text titleText = newJobInfo.transform.Find("Title").GetComponent<TMP_Text>();
            TMP_Text recruiterText = newJobInfo.transform.Find("Recruiter").GetComponent<TMP_Text>();
            TMP_Text healthLossText = newJobInfo.transform.Find("HealthLoss").GetComponent<TMP_Text>();
            TMP_Text payText = newJobInfo.transform.Find("Pay").GetComponent<TMP_Text>();

            // 텍스트 설정
            titleText.text = jobList[i].title;
            recruiterText.text = jobList[i].recruiter;

            // 건강 손실 및 급여 설정
            if (i == 0)
            {
                healthLossText.text = "체력1 소모";
                payText.text = Random.Range(3, 6).ToString() + "만원";
            }
            else if (i == 1)
            {
                healthLossText.text = "체력2 소모";
                payText.text = Random.Range(9, 12).ToString() + "만원";
            }
            else if (i == 2)
            {
                healthLossText.text = "체력3 소모";
                payText.text = Random.Range(17, 21).ToString() + "만원";
            }
        }
    }
}