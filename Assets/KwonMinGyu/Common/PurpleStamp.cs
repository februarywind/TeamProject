using System.Collections.Generic;
using UnityEngine;

public class PurpleStamp : MonoBehaviour
{
    [SerializeField] GameObject _teleportPoint;
    [SerializeField] GameObject _MarkerPrefab;
    [SerializeField] bool _isActive;
    [SerializeField] Vector3[] _teleportSpots;
    [SerializeField] LayerMask _teleportLayer;

    private CubeMove _cubeMove;
    private CubeChecker _cubeChecker;
    private List<GameObject> _markerPool = new();

    int index = 0;
    int dir = 1;

    private void Start()
    {
        _cubeMove = CubeMove.Instance;
        _cubeChecker = CubeChecker.Instance;
    }
    private void Update()
    {
        if (!_cubeChecker.IsStampUse) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isActive)
            {
                PurpleUnActivate();
            }
            else
            {
                PurpleActivate();
            }
        }
        if (!_isActive) return;

        // WASD로 인덱스 변경
        if (Input.GetKeyDown(KeyCode.W)) { index += -7; dir = -1; }
        if (Input.GetKeyDown(KeyCode.A)) { index += -1; dir = -1; }
        if (Input.GetKeyDown(KeyCode.S)) { index += 7; dir = 1; }
        if (Input.GetKeyDown(KeyCode.D)) { index += 1; dir = 1; }
        while (true)
        {
            if (index > 48) index -= 49;
            if (index < 0) index = 48;
            if (!(_teleportSpots[index] == Vector3.zero)) break;
            index += 1 * dir;
        }
        _teleportPoint.transform.position = _teleportSpots[index];

        // 스페이스로 타겟 위치로 이동
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cubeMove.transform.position = _teleportSpots[index] + Vector3.up * 0.5f;
            PurpleUnActivate();
        }
    }

    private void SearchTeleportSpot()
    {
        _teleportSpots = new Vector3[49];
        Vector3 startPos = _cubeMove.transform.position + new Vector3(-3, 3, 3);
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (Physics.Raycast(startPos + new Vector3(j, 0, -i), Vector3.down, out RaycastHit hit, 7, _teleportLayer))
                {
                    _teleportSpots[i * 7 + j] = hit.point;
                    MarkerPool(hit.point);
                }
            }
        }
    }
    private void PurpleActivate()
    {
        // 능력 비활성화
        _isActive = true;
        // 큐브 이동 가능
        _cubeMove.IsRolling = true;
        // 텔레포트 위치 표식 비활성화
        _teleportPoint.SetActive(true);

        SearchTeleportSpot();
    }
    private void PurpleUnActivate()
    {
        // 능력 비활성화
        _isActive = false;
        // 큐브 이동 가능
        _cubeMove.IsRolling = false;
        // 텔레포트 위치 표식 비활성화
        _teleportPoint.SetActive(false);

        foreach (var item in _markerPool)
        {
            item.SetActive(false);
        }
    }

    private void MarkerPool(Vector3 _pos)
    {
        foreach (var item in _markerPool)
        {
            if (item.activeSelf) continue;
            item.transform.position = _pos;
            item.SetActive(true);
            return;
        }
        _markerPool.Add(Instantiate(_MarkerPrefab, _pos, transform.rotation));
    }
}
