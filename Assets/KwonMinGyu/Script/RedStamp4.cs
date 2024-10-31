using System.Collections.Generic;
using UnityEngine;

public class RedStamp4 : MonoBehaviour
{
    // 스탬프 제어를 위한 스크립트
    [SerializeField] private CubeChecker4 _cubeChecker4;

    // 큐브 이동 제어를 위한 스크립트
    [SerializeField] private CubeMove4 _cubeMove4;

    // 스탬프 능력 사용 시 바닥이 될 4개의 오브젝트와 위치
    [SerializeField] GameObject[] _redGrounds;
    private Vector3[] _redGroundPos = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    // 큐브가 _redGrounds 에 닿을 때 그 자리를 대체할 프리팹
    [SerializeField] private GameObject _useGround;

    // 빨간 스탬프 능력 활성화 여부
    [SerializeField] private bool _active;

    // _redGrounds의 생성 제한에 사용할 변수 
    [SerializeField] private bool _groundSet;

    // 최대 능력 사용 횟수
    [SerializeField] private int maxRedGround;

    // 능력 사용 횟수를 저장할 변수
    [SerializeField] private int useRedGround;

    // 능력 사용 위치를 저장할 변수와 오브젝트
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private GameObject _startObj;

    // 오브젝트 풀을 위한 리스트
    [SerializeField] private List<GameObject> pool;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 능력이 활성화 중 이라면 비활성화 함수 실행
            if (_active)
                RedDisable();
            else
            {
                // 능력 활성화
                _active = true;

                // cubeChecke를 비 활성화 함으로 다른 능력 사용을 방지
                _cubeChecker4.enabled = false;

                // 능력 사용 위치 저장
                _startPos = _cubeMove4.transform.position;

                // 시작 위치 오브젝트
                _startObj.transform.position = _startPos;
                _startObj.SetActive(true);
            }
        }
        // 능력이 활성화 상태 && 큐브가 이동을 시작할 때
        if (_active && _cubeMove4.IsRolling)
        {
            // 큐브의 이번 움직임으로 바닥을 생성 했다면 리턴
            if (_groundSet) return;

            // 능력 사용 횟수를 초과 했다면 리턴
            if (!(useRedGround < maxRedGround)) return;
            // 능력 사용
            RedActive();
        }
        // 큐브 이동 종료로 다시 바닥을 생성할 수 있음
        if (!_cubeMove4.IsRolling) _groundSet = false;
    }
    private void RedActive()
    {
        // 바닥을 큐브 아래칸 주위에 배치
        for (int i = 0; i < 4; i++)
            _redGrounds[i].transform.position = _cubeMove4.transform.position + _redGroundPos[i] + Vector3.down * 2;
        // 능력 사용 체크
        _groundSet = true;
    }

    // 해당 함수는 _redGrounds와 플레이어의 물리적 충돌 시 활성화 됨
    public void UseRedGround(Transform _transform)
    {
        // 오브젝트 풀로 충돌 위치를 대체할 바닥을 생성
        Pool(_useGround, _transform);
        // 바닥 생성 카운트
        useRedGround++;
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
    private void RedDisable()
    {
        // 다른 스탬프로 전환이 가능하도록 cubeChecker 활성화
        _cubeChecker4.enabled = true;

        // 능력 미 사용 상태
        _active = false;

        // 큐브 위치를 시작 위치로
        _cubeMove4.transform.position = _startPos;

        // 시작 위치 오브젝트 비활성화
        _startObj.SetActive(false);

        // 능력 횟수 초기화
        useRedGround = 0;

        // 생성된 바닥 비활성화
        foreach (var item in pool)
        {
            item.SetActive(false);
        }
    }


    // 레이캐스트를 지속적으로 사용하는 _redGrounds는 RedStamp가 활성화 일 때만 활성화
    private void OnEnable()
    {
        foreach (var item in _redGrounds)
        {
            item.SetActive(true);
        }
    }
    private void OnDisable()
    {
        foreach (var item in _redGrounds)
        {
            item.SetActive(false);
        }
    }
}
