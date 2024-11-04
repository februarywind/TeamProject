using UnityEngine;

public class CubeMoveRay : MonoBehaviour
{
    // 큐브 제어를 위한 스크립트
    [SerializeField] private CubeMove _cubeMove;

    // 해당 객체가 관리할 방향
    [SerializeField] private CubePos _blockingDir;

    // 바닥이 가지는 레이를 입력
    [SerializeField] private LayerMask _layerMask;

    // 벽면이 가지는 레이를 입력 해당 레이어는 큐브가 통과 못함
    [SerializeField] private LayerMask _WallLayer;

    // 큐브기준 오프셋
    [SerializeField] private Vector3 _offSet;

    // 트리거 상태 일 때 트루
    [SerializeField] bool _wallTrigger;
    [SerializeField] Transform _transform;

    RedStamp _redStamp;
    private void Start()
    {
        _redStamp = CubeChecker.Instance.GetComponent<RedStamp>();
    }

    private void OnTriggerStay(Collider other)
    {
        // 맵이 가지는 레이어(Default)가 아니라면 리턴
        if ((_WallLayer & (1 << other.gameObject.layer)) == 0) return;

        // 트리거 되면 해당 방향을 막음
        _cubeMove.BlockingDir[(int)_blockingDir] = _blockingDir;

        // 트리거 진입 상태
        _wallTrigger = true;

        _transform = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        // 맵이 가지는 레이어(Default)가 아니라면 리턴
        if ((_WallLayer & (1 << other.gameObject.layer)) == 0) return;

        // 벽면에서 빠져 나왔음으로 BlockingDir을 None으로 변경
        _cubeMove.BlockingDir[(int)_blockingDir] = CubePos.None;

        // 트리거 벗어남
        _wallTrigger = false;
    }

    private void Update()
    {
        // 큐브 위치 + 오프셋 위치로 이동
        transform.position = _cubeMove.transform.position + _offSet;

        // 트리거 중 이라면 리턴
        if (_wallTrigger) return;

        // 바닥이 감지 안돼면 해당 방향을 블로킹
        _cubeMove.BlockingDir[(int)_blockingDir] = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _hit, 15f, _layerMask) ? CubePos.None : _blockingDir;
    }

    // 트리거 중 오브젝트가 파괴 혹은 비활성화 시 Exit을 대체하는 함수
    public void TriggerExit()
    {
        _cubeMove.BlockingDir[(int)_blockingDir] = CubePos.None;
        _wallTrigger = false;
    }
}
