using System.Collections.Generic;
using UnityEngine;

public class RedStamp : MonoBehaviour
{
    // 스탬프 제어를 위한 스크립트
    [SerializeField] private CubeChecker _cubeChecker;

    // 큐브 이동 제어를 위한 스크립트
    [SerializeField] private CubeMove _cubeMove;

    // 스탬프 능력 사용 시 임시로 바닥이 될 오브젝트
    [SerializeField] GameObject _redTrigger;

    // 기본 바닥의 레이어를 입력
    [SerializeField] private LayerMask _GroundLayer;

    // 큐브가 _redTrigger 에 닿을 때 그 자리를 대체할 프리팹
    [SerializeField] private GameObject _redGround;

    // 빨간 스탬프 능력 활성화 여부
    public bool _active { get; private set; }

    // 최대 능력 사용 횟수
    [SerializeField] private int maxRedGround;

    // 능력 사용 횟수를 저장할 변수
    [SerializeField] private int useRedGround;

    // 능력 사용 위치를 저장할 변수와 오브젝트
    [SerializeField] private Vector3 _startPos;
    [SerializeField] Quaternion _startRot;
    [SerializeField] private GameObject _startObj;

    // 오브젝트 풀을 위한 리스트
    [SerializeField] private List<GameObject> pool;

    // 낙하가 가능하도록 하는 오브젝트
    [SerializeField] private GameObject _plane;

    // 트리거 초기화를 위한 CubeMoveRay
    [SerializeField] private CubeMoveRay[] _cubeMoveRays;

    private void Update()
    {
        if (!CubeChecker.Instance.IsStampUse) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 능력이 활성화 중 이라면 비활성화 함수 실행
            if (_active)
                RedDisable(true);
            else
            {
                // 낭떠러지가 아니라면 능력 사용 불가
                if (!IsCliff()) return;

                // 능력 활성화
                _active = true;

                // 빨강 모드 활성화
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.RedPurpleStamp);

                // cubeChecke를 비 활성화 함으로 다른 능력 사용을 방지
                _cubeChecker.enabled = false;

                // 능력 사용 위치 저장
                _startPos = _cubeMove.transform.position;
                _startRot = _cubeMove.transform.rotation;

                // 시작 위치 오브젝트
                _startObj.transform.position = _startPos;
                _startObj.SetActive(true);

                // 낙하 유도 오브젝트 활성화
                _plane.SetActive(true);
            }
        }
    }
    public void RedActive()
    {
        // 바닥을 큐브 아래칸에 배치
        _redTrigger.transform.position = _cubeMove.transform.position + Vector3.down * 2;
    }

    // 해당 함수는 _redGrounds와 플레이어의 물리적 충돌 시 활성화 됨
    public void UseRedGround(Transform _transform)
    {
        // 오브젝트 풀로 충돌 위치를 대체할 바닥을 생성
        Pool(_redGround, _transform);
        // 빨강 모드 발판 생성 효과음 
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Scaffolding);
        // 바닥 생성 카운트
        useRedGround++;
        if (useRedGround == maxRedGround) _plane.SetActive(false);
    }

    private void Pool(GameObject _prefab, Transform _transform)
    {
        foreach (var item in pool)
        {
            if (item.activeSelf) continue;
            item.transform.position = _transform.position;
            item.SetActive(true);
            return;
        }
        pool.Add(Instantiate(_prefab, _transform.position, _transform.rotation));
    }

    // 빨간 스탬프 능력 해제
    private void RedDisable(bool reset)
    {
        // 빨강 모드 종료 효과음
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ScaffoldingEnd);

        // 낙하 유도 오브젝트 비활성화
        _plane.SetActive(false);

        // 레드 트리거 비활성화
        _redTrigger.SetActive(false);

        // 다른 스탬프로 전환이 가능하도록 cubeChecker 활성화
        _cubeChecker.enabled = true;

        // 능력 미사용 상태
        _active = false;

        // 큐브 위치를 시작 위치로
        if (reset)
        {
            _cubeMove.transform.position = _startPos;
            _cubeMove.transform.rotation = _startRot;
            _cubeMove.FallCheck();
        }

        // 시작 위치 오브젝트 비활성화
        _startObj.SetActive(false);

        // 능력 횟수 초기화
        useRedGround = 0;

        // 생성된 바닥 비활성화
        foreach (var item in pool)
            item.SetActive(false);

        // 블로킹 초기화
        _cubeMove.BlockingReset();

        // 트리거 초기화
        foreach (var item in _cubeMoveRays)
            item.TriggerExit();
    }
    public void GroundCheck()
    {
        if (!Physics.Raycast(_cubeMove.transform.position, Vector3.down, out RaycastHit hit, 1, _GroundLayer)) return;
        if (useRedGround == 0 || (_GroundLayer & (1 << hit.transform.gameObject.layer)) == 0) return;
        RedDisable(false);
    }

    // 레이캐스트를 지속적으로 사용하는 _redGrounds는 RedStamp가 활성화 일 때만 활성화
    private void OnEnable()
    {
        _redTrigger.SetActive(true);
    }

    // 방향 지정용 배열
    private Vector3[] _redRayVec = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    // 4방향 아래로 레이를 날려 하나라도 인식하지 못한다면 낭떠러지
    private bool IsCliff()
    {
        foreach (var item in _redRayVec)
        {
            if (!Physics.Raycast(transform.position + item, Vector3.down,out RaycastHit hit,  2f, _GroundLayer))
                return true;
        }
        return false;
    }
}
