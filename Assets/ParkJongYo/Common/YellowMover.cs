using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class YellowMover : MonoBehaviour
{
    [SerializeField] float moveDistance = 4f; // 이동 거리
    [SerializeField] float moveSpeed = 20f; // 이동 속도

    private bool isMoving = false; // 이동 중인지 여부
    private Transform playerTransform; // 플레이어 오브젝트의 transform
    [SerializeField] private CubeMove cubeMove; // CubeMove 스크립트 참조
    [SerializeField] private CubeChecker cubeChecker;

    // Raycast가 검사하는 레이어
    [SerializeField] private LayerMask _yellowMask;

    // 인스펙터에서 입력되는 거리를 저장 함
    private float _moveDistance;

    [Header("YellowChecker_Manager")]
    [SerializeField] YellowChecker yellowPrefab;       // 갈 수 있는지 확인해주는 체커 프리팹
    [SerializeField] YellowChecker[] yellowCheckers;   // 최대 갈 수 있는 거리만큼 체커를 담아두기
    [SerializeField] bool[] result;                     // 인스펙터 창으로 갈 수 있는 거리 디버깅 용도

    [Header("Stamps")]
    [SerializeField] Rigidbody[] stampRigidbodies;      // 스탬프들의 중력들

    void Start()
    {

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform; // 플레이어 오브젝트의 transform을 가져옴
        }
        _moveDistance = moveDistance;

        yellowCheckers = new YellowChecker[(int)_moveDistance];    // 갈 수 있는 거리만큼 생성
        result = new bool[yellowCheckers.Length];                   // 인스펙터 창에서 디버깅 용도로 사용

        for (int i = 0; i < _moveDistance; i++)
        {
            yellowCheckers[i] = Instantiate(yellowPrefab);
            yellowCheckers[i].gameObject.SetActive(false);
        }

    }

    void Update()
    {
        // 플레이어 오브젝트가 존재할 때만 이동 처리
        if (playerTransform != null && !isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 카메라가 바라보는 방향으로 이동 벡터 계산
                Vector3 moveDirection = Camera.main.transform.forward; // 카메라 방향
                moveDirection.y = 0; // Y 축 이동 방지

                // X 또는 Z 방향으로만 이동하도록 제한
                if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
                {
                    moveDirection.z = 0; // Z 방향 제거
                }
                else
                {
                    moveDirection.x = 0; // X 방향 제거
                }

                moveDirection.Normalize(); // 방향 정규화

                // Raycast로 이동 거리를 변경함 -> 이동 거리 설정을 코루틴에서 관리
                // moveDistance = StartRay(moveDirection);
                // moveDirection *= moveDistance; // 이동 거리 설정

                StartCoroutine(SmoothMove(moveDirection)); // 이동
            }
        }
    }

    IEnumerator SmoothMove(Vector3 direction)
    {
        // 이동 중 큐브의 회전을 막음
        cubeMove.IsRolling = true;

        // 거리 계산 부분
        moveDistance = 0;
        for (int index = 1; index <= yellowCheckers.Length; index++)
        {
            // 플레이어가 바라보고 있는 방향에서 최대 이동 거리만큼 하나씩 배치                                                                
            yellowCheckers[index - 1].gameObject.transform.position = new Vector3(playerTransform.position.x + (index * direction.x),   // 플레이어가 바라보고 있는 방향에서 최대 이동 거리만큼 하나씩 배치
                                                                                  playerTransform.position.y + 0.1f, // 너무 가운데에 있으면 다른 콜라이더에 닿을까봐 높이를 살짝 높임      
                                                                                  playerTransform.position.z + (index * direction.z));
            yellowCheckers[index - 1].gameObject.SetActive(true);   // 해당 오브젝트를 활성화 -> Collider 작동
            yellowCheckers[index - 1].CheckRay();                   // 해당 오브젝트의 아래 방향으로 Ray 발싸 -> 아래에 갈 수 있는 땅이 있는지 확인
        }

        yield return new WaitForFixedUpdate();

        for (int index = 0; index < result.Length; index++)
        {
            result[index] = yellowCheckers[index].CanMove;         // 디버깅 용도로 담기
            if (result[index]) moveDistance++;                     // 만약 갈 수 있으면 갈 수 있는 거리 +1
            else break;                                            // 못가면 현재 갈 수 있는 만큼만 갈 수 있으니 더 이상 탐색 안하기
        }

        yield return null;

        for (int index = 0; index < yellowCheckers.Length; index++)
        {
            yellowCheckers[index].gameObject.SetActive(false);      // 오브젝트를 다 사용했으니 비활성화
        }

        direction *= moveDistance; // 이동 거리 설정

        isMoving = true; // 이동 시작
        int yellowCheckerindex = 0;
        Vector3 startPosition = playerTransform.position; // 플레이어 오브젝트의 시작 위치
        Vector3 endPosition = startPosition + direction; // 끝 위치 계산

        // stamp의 rigidvody 시작 끝 위치 설정
        Vector3[] rigidStartPositions = new Vector3[stampRigidbodies.Length];
        Vector3[] rigidEndPositions = new Vector3[stampRigidbodies.Length];

        for(int index = 0; index < stampRigidbodies.Length; index++)
        {
            rigidStartPositions[index] = stampRigidbodies[index].position;
            rigidEndPositions[index] = rigidStartPositions[index] + direction;
        }

        float elapsedTime = 0;

        float pastGap;  // 능력 사용 중 파괴할 오브젝트와의 거리
        float curGap;  // 능력 사용 중 파괴할 오브젝트와의 거리

        Vector3 pastPos;
        Vector3 curPos;

        // 디버깅용 로그
        // Debug.Log($" first :{yellowCheckerindex} vector : {Vector3.Magnitude(yellowCheckers[yellowCheckerindex].transform.position - playerTransform.position)}");
        yellowCheckers[yellowCheckerindex++].DestroyTarget(); // 1번 째 체커의 오브젝트 파괴
        while (elapsedTime < moveDistance / moveSpeed)
        {

            pastPos = playerTransform.position;

            playerTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / (moveDistance / moveSpeed)); // 플레이어 오브젝트 위치 업데이트
                                                                                                                           
            yield return null;

            // Stamp 옮기는 부분
            for (int index = 0; index < stampRigidbodies.Length; index++)
            {
                stampRigidbodies[index].position = Vector3.Lerp(rigidStartPositions[index], rigidEndPositions[index], elapsedTime / (moveDistance / moveSpeed));
            }

            curPos = playerTransform.position;

            // 파괴할 오브젝트와의 거리 계산
            pastGap = Vector3.Magnitude(yellowCheckers[yellowCheckerindex].transform.position - pastPos);
            curGap = Vector3.Magnitude(yellowCheckers[yellowCheckerindex].transform.position - curPos);

            if (1f <= pastGap && curGap < 1f)
            { 
                //디버깅용 로그
                // Debug.Log($" in loop :{yellowCheckerindex} vector : {curGap}"); 

                yellowCheckers[yellowCheckerindex].DestroyTarget(); // 2 ~ n - 1 번째 체커의 오브젝트 파괴
                // 만약 현재 체커가 마지막 바로 전이 아니라면
                if (yellowCheckerindex < yellowCheckers.Length - 1) yellowCheckerindex++;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 마지막 위치 설정
        for (int index = 0; index < stampRigidbodies.Length; index++)
        {
            stampRigidbodies[index].position = rigidEndPositions[index]; // 각 스탬프의 마지막 위치 설정
        }

        playerTransform.position = endPosition;  // 플레이어의 마지막 위치 설정


        //디버깅용 로그
        // Debug.Log($"last : {yellowCheckerindex}");
        yellowCheckers[yellowCheckers.Length - 1].DestroyTarget(); // 마지막 n번째 체커의 오브젝트 파괴

        isMoving = false; // 이동 완료

        // cubeChecker를 큐브 위로 이동
        cubeChecker.RePosition(cubeMove.transform.position);
        // rigid를 움직여서 충돌을 인식 못하수도 있어서 한번 껏다 켜보기
        cubeChecker.gameObject.SetActive(false);
        cubeChecker.gameObject.SetActive(true);

        // 큐브 회전이 가능 하도록 원복
        cubeMove.IsRolling = false;
    }

    private float StartRay(Vector3 _dir)
    {
        // 노랑색 스템프의 이동 방향으로 Raycast
        if (Physics.Raycast(transform.position, _dir, out RaycastHit hit, _moveDistance + 1, _yellowMask))
        {
            // 검사되는 물체가 "CrackedRock" 태그를 가진 경우
            if (hit.transform.CompareTag("CrackedRock"))
            {
                StartCoroutine(DestroyCrackedRock(hit.transform.gameObject)); // CrackedRock 파괴 코루틴 시작
                return _moveDistance; // CrackedRock 파괴 후에도 원래 이동 거리(4)로 설정
            }

            // CrackedRock이 아닌 다른 장애물과 맞닿아 있을 때는 해당 위치에서 멈춤
            return Mathf.Floor(Vector3.Distance(transform.position, hit.point));
        }

        // 검사되는 물체가 없다면 최대 거리로 리턴
        return _moveDistance;
    }

    private IEnumerator DestroyCrackedRock(GameObject crackedRock)
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        Destroy(crackedRock); // CrackedRock 파괴
    }

}