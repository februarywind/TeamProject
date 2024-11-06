using System.Collections.Generic;
using UnityEngine;

public class PurpleStamp4 : MonoBehaviour
{
    // 큐브 이동 제어를 위한 스크립트
    [SerializeField] CubeMove4 cubeMove;

    // 텔레포트가 가능한 레이어
    [SerializeField] LayerMask _teleportLayer;

    // 텔레포트 할 위치와 가능한 위치 표식
    [SerializeField] GameObject _teleportTarget;

    // 텔레포트가 가능한 위치를 저장할 리스트
    [SerializeField] List<Vector3> _teleportPoints;
    float index = 0;

    // 능력 활성화 확인 변수
    [SerializeField] bool _active;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_active)
                PurpleUnActivate();
            else
                PurpleActivate();
        }
        // 능력이 비활성화 일 때 리턴
        if (!_active) return;

        // 스페이스로 타겟 위치로 이동
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cubeMove.transform.position = _teleportTarget.transform.position + Vector3.up * 0.5f;
            PurpleUnActivate();
        }

        // WASD로 인덱스 변경
        if (Input.GetKeyDown(KeyCode.W)) index += 7;
        if (Input.GetKeyDown(KeyCode.A)) index += -1;
        if (Input.GetKeyDown(KeyCode.S)) index += -7;
        if (Input.GetKeyDown(KeyCode.D)) index += 1;

        // 리스트 이탈 방지
        index %= _teleportPoints.Count;
        if (index < 0) index = _teleportPoints.Count - 1;

        // 텔레포트 타겟의 위치 변경
        _teleportTarget.transform.position = _teleportPoints[(int)index];
    }

    private void PurpleActivate()
    {
        // 능력 활성화
        _active = true;
        // 큐브 이동 막기
        cubeMove.IsRolling = true;
        // 레이 발사
        PurpleRay();
        // 텔레포트 위치 표식 활성화 후 이동
        _teleportTarget.SetActive(true);
        _teleportTarget.transform.position = _teleportPoints[1];
    }

    private void PurpleUnActivate()
    {
        // 능력 비활성화
        _active = false;
        // 큐브 이동 가능
        cubeMove.IsRolling = false;
        // 텔레포트 위치 표식 비활성화
        _teleportTarget.SetActive(false);
    }

    void PurpleRay()
    {
        // 리스트 클리어
        _teleportPoints.Clear();
        // 시작 위치 설정
        Vector3 startPos = cubeMove.transform.position + new Vector3(-3, 3, -3);
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (Physics.Raycast(startPos + new Vector3(i, 0, j), Vector3.down, out RaycastHit hit, 7, _teleportLayer))
                    _teleportPoints.Add(hit.point);
            }
        }
    }
}
