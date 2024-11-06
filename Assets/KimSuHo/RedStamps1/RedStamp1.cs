using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStamp1 : MonoBehaviour
{
    [SerializeField] GameObject hoCube; // 플레이어 분신
    public stepPool1 stepPool; // 오브젝트 풀
    public Transform spawnPoint; // 발판 생성 위치
    public int maxSteps = 6; // 최대 발판 수
    private int currentSteps = 0; // 현재 발판 수
    private bool abilityActive = false; // 능력 활성화 상태
    private Vector3 initialPosition; // 처음 능력 사용했던 지점
    private List<GameObject> activeSteps = new List<GameObject>(); // 현재 활성화된 발판 목록

    void Start()
    {
        hoCube.SetActive(false);
    }

    void Update()
    {
        // 발판 생성 위치 업데이트
        spawnPoint.transform.position = transform.position + Vector3.down * 0.5f;

        // 능력 발동
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            ActivateAbility();
        }

        // 능력 취소
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            CancelAbility();
        }

        // 입력 체크 (이동)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 발판 생성 조건 추가
        if (abilityActive && (horizontalInput != 0 || verticalInput != 0) && currentSteps < maxSteps)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(GenerateStep()); // 발판 생성
            }
        }

        void ActivateAbility()
        {
            if (!abilityActive)
            {
                abilityActive = true; // 능력 활성화
                currentSteps = 0; // 발판 수 초기화
                initialPosition = transform.position; // 초기 위치 저장
                hoCube.SetActive(true);
                hoCube.transform.position = transform.position;
            }
        }

        void CancelAbility()
        {
            if (abilityActive)
            {
                // 능력 취소 후 초기 위치로 돌아가기
                transform.position = initialPosition; // 초기 위치로 돌아가기
                hoCube.SetActive(false);

                // 발판 반환
                foreach (var step in activeSteps)
                {
                    stepPool.ReturnStep(step);
                }

                // 상태 초기화
                activeSteps.Clear(); // 활성화된 발판 목록 초기화
                abilityActive = false; // 능력 비활성화
                currentSteps = 0; // 발판 수 초기화
            }
        }

        IEnumerator GenerateStep()
        {
            // 발판 가져오기
            GameObject step = stepPool.GetStep();
            if (step != null)
            {
                // 발판 위치 설정 (현재 위치 아래쪽으로)
                step.transform.position = spawnPoint.position + Vector3.down * 1f * currentSteps;
                currentSteps++;
                activeSteps.Add(step); // 활성화된 발판 목록에 추가
            }

            yield return null; // 발판 생성 후 다음 프레임으로 이동
        }
    }
}