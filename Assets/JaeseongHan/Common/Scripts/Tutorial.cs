using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] TutorialType tutorialType; // 적용할 튜토리얼 타입
    [SerializeField] BoxCollider boxCollider;   // 튜토리얼이 나오는 위치
    [Header("Text")]
    [SerializeField] Canvas canvas;             // 튜토리얼 툴팁이 나오는 위치
    [SerializeField] TextMeshProUGUI textArea;  // 튜토리얼의 툴팁

    // 튜토리얼 타입
    public enum TutorialType
    { 
        None,               // 없음
        Start,              // stage1 시작 직후 움직임
        Blue,               // 클로버 스탬프 최초 획득
        MoveMouse,          // 첫 퍼즐 통과 직후
        Water,              // 물 타일 최초 조우
        BlueAbility,        // 물 타일 2번째 조우 직전
        Reset,              // 리셋 타일 최초 조우
        Yellow,             // 신발 스탬프 최초 획득_이동
        YellowAbility,      // 신발 스탬프 최초 획득_블록 파괴
        Red,                // 모루 스탬프 최초 획득
        RedAbility,         // 두 번째 퍼즐구간 모루 스탬프 획득
        Purple,             // 보석 스탬프 최초 획득
        Size                // 튜토리얼 타입의 총 크기
    }

    private StringBuilder sb;           // 튜토리얼의 Text를 입력할 stringBuilder
    private Coroutine coroutine;        // 튜토리얼 UI 보이게 설정 
    private int routineTime;            // 보여줄 횟수

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        sb = new StringBuilder();
    }

    private void Start()
    {
        if(tutorialType.Equals(TutorialType.None)) boxCollider.enabled = false;
    }

    private void SetText(int n = 0)
    {
        sb.Clear();

        switch (tutorialType)
        {
            case TutorialType.Start:
                sb.Append("W, A, S, D 키로 큐브를 이동시킬 수 있습니다.");
                break;
            case TutorialType.Blue:
                sb.Append("스탬프를 획득한 큐브의 면을 스탬프 체크 타일에 \n맞추어 퍼즐을 풀 수 있습니다.");
                break;
            case TutorialType.MoveMouse:
                sb.Append("마우스를 움직여 시야를 움직일 수 있습니다.");
                break;
            case TutorialType.Water:
                sb.Append("물 타일은 맞닿은 큐브의 면에 있는 스탬프를 제거합니다.");
                break;
            case TutorialType.BlueAbility:
                if(n == 0) sb.Append("스탬프는 고유의 능력을 가지고 있습니다.\n\n큐브의 윗 면에 스탬프가 위치하였을 때 사용할 수 있습니다.");
                if(n == 1) sb.Append("클로버 스탬프는 E키를 누른 채 A와 D키를 이용해 \n\n제자리에서 측면을 회전시킬 수 있습니다.");
                break;
            case TutorialType.Reset:
                sb.Append("리셋 타일은 큐브의 모든 면의 스탬프를 제거합니다.");
                break;
            case TutorialType.Yellow:
                sb.Append("신발 스탬프는 마우스 왼쪽 버튼을 클릭해 \n\n플레이어가 바라보는 방향으로 큐브를 돌진시킬 수 있습니다.");
                break;
            case TutorialType.YellowAbility:
                sb.Append("신발 스탬프를 사용하여 일부 구조물을 파괴할 수 있습니다.");
                break;
            case TutorialType.Red:
                sb.Append("모루 스탬프는 E키를 사용해 공중에서 계단식으로 발판을 생성할 수 있습니다.");
                break;
            case TutorialType.RedAbility:
                sb.Append("모루 스탬프 사용 중 E키를 사용해 능력을 사용한 자리로 돌아올 수 있습니다.");
                break;
            case TutorialType.Purple:
                if (n == 0) sb.Append("보석 스탬프는 E키를 사용해 큐브를 중심으로 하여 일정 범위의 이동 가능 구역을 생성합니다.");
                if (n == 1) sb.Append(" W, A, S, D키로 좌표를 지정하여, SpaceBar를 눌러 지정한 타일로 순간이동할 수 있습니다. ");
                break;
        }

        textArea.SetText(sb.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SetText();
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(ShowTutorialUI());

            boxCollider.enabled = false;
        }
    }

    private IEnumerator ShowTutorialUI()
    {
        canvas.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(5f);

        if(tutorialType.Equals(TutorialType.BlueAbility) || tutorialType.Equals(TutorialType.Purple))
        {
            // 딜레이 이후
            SetText(1);
            canvas.gameObject.SetActive(true);

            yield return new WaitForSeconds(5f);
        }
        canvas.gameObject.SetActive(false);
    }
}
