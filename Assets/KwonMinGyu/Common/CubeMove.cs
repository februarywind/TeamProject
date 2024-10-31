using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public enum CubePos
{
    Up, Down, Right, Left, None
}
public class CubeMove : MonoBehaviour
{
    // 큐브 회전속도
    [SerializeField] private float _rotationSpeed;

    // 큐브 윗면 체크 및 바닥 체크를 위한 CubeChecker 스크립트
    [SerializeField] private GameObject _cubeCheckerObj;
    [SerializeField] private CubeChecker _cubeChecker;
    [SerializeField] private BoxCollider[] stampPoints; // 스탬프 콜라이더들

    // 큐브 낙하를 위한 Rigidbody
    [SerializeField] private Rigidbody _rigidbody;

    // 카메라 방향을 구하기 위한 CameraMove 스크립트
    [SerializeField] private CameraMove _cameraMove;
    private CameraPos _cameraPos;

    // 카메라 뱡향에 따른 큐브 회전을 위한 필드
    private Vector3[] _moveDir = new Vector3[4];
    private CubePos _cubePos;
    private Bounds bound;

    // true일 때 플레이어의 입력이 막힘
    public bool IsRolling;

    // 경사로 이동속도
    [SerializeField] private float _slopeSpeed;

    // 경사로 확인 필드, SlopeBlack와 Trigger될 때 자동 입력됨
    public bool IsSlopeForward;
    public CubePos SlopeDir;
    public Vector2 SlopeDistance;

    // 특정 칸에서 큐브 이동을 막기 위한 필드, CubeMoveBlocking와 Trigger될 때 자동 입력됨
    public bool IsBlockingForward;
    public CubePos[] BlockingDir = { CubePos.None, CubePos.None, CubePos.None, CubePos.None };

    void Start()
    {
        // bound.size값으로 콜라이더의 월드기준 size를 구할 수 있음
        // 아래 값들은 RotateAround의 회전 기준점이 됨
        bound = GetComponent<BoxCollider>().bounds;
        _moveDir[0] = new Vector3(0, -bound.size.y / 2, bound.size.z / 2);
        _moveDir[1] = new Vector3(0, -bound.size.y / 2, -bound.size.z / 2);
        _moveDir[2] = new Vector3(bound.size.x / 2, -bound.size.y / 2, 0);
        _moveDir[3] = new Vector3(-bound.size.x / 2, -bound.size.y / 2, 0);

        // Rigidbody 기본 설정
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll - 4;

        // 시작 낙하를 위한 바닥 체크
        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    void Update()
    {
        _cubeChecker.RePosition(transform.position);

        float _HMove = Input.GetAxisRaw("Horizontal");
        float _VMove = Input.GetAxisRaw("Vertical");

        if (_HMove == 0 && _VMove == 0 || IsRolling) return;

        // 처음 큐브 위치를 기준으로 카메라 방향을 구함
        _cameraPos = _cameraMove.CameraPosition();

        // Move Forward
        if (_VMove == 1)
        {
            // 카메라 방향에 따라 회전 방향을 변경
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Down;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Up;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Left;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Right;
                    break;
            }
        }

        // Move Backwards
        else if (_VMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Up;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Down;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Right;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Left;
                    break;
            }
        }

        // Move Right
        else if (_HMove == 1)
        {
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Left;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Right;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Up;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Down;
                    break;
            }
        }

        // Move Left
        else if (_HMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Right;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Left;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Down;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Up;
                    break;
            }
        }

        // 이동 못하는 방향으로 이동 시 리턴
        if (BlockingDir.Contains(_cubePos))
            return;

        // 경사로 앞에서 경사로 쪽으로 이동시 경사로 이동 코루틴 실행
        if (IsSlopeForward && SlopeDir == _cubePos)
        {
            StartCoroutine(SlopeMove(_cubePos));
            return;
        }

        StartCoroutine(Roll(_cubePos));

    }

    IEnumerator Roll(CubePos cubePos)
    {
        // 중복 이동 방지
        IsRolling = true;

        // 이동 중 스탬프 사용을 막기 위해 CubeCheck 비활성화
        _cubeCheckerObj.SetActive(false);

        // 회전 방향에 따른 기준점을 구함
        Vector3 positionToRotation = _moveDir[(int)cubePos];
        Vector3 point = transform.position + positionToRotation;

        // Vector3.up와 회전 방향의 외적을 구해서 회전 축으로 한다
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        // 90도 만큼 회전
        float angle = 0;
        while (angle < 90f)
        {
            // 1초당 _rotationSpeed 만큼 회전
            float angleSpeed = Time.deltaTime * _rotationSpeed;

            // (기준점, 회천축, 회전값)으로 회전
            transform.RotateAround(point, axis, angleSpeed);

            // 회전값 저장
            angle += angleSpeed;

            yield return null;
        }

        // 회전이 90도보다 적거나 많이 될 때 90도로 조정
        transform.RotateAround(point, axis, 90 - angle);


        // CubeCheck를 큐브 위로 이동 후 CubeCheck 활성화
        _cubeChecker.RePosition(transform.position);
        _cubeCheckerObj.SetActive(true);

        // 회전 종료, 이동 가능
        IsRolling = false;

        // 회전 후 공중이라면 추락
        FallCheck();
    }

    IEnumerator SlopeMove(CubePos cubePos)
    {
        // 중복 이동 방지
        IsRolling = true;

        // 이동 중 스탬프 사용을 막기 위해 CubeCheck 비활성화
        _cubeCheckerObj.SetActive(false);

        // 회전 방향에 따른 기준점을 구함
        Vector3 positionToRotation = _moveDir[(int)cubePos];
        Vector3 point = transform.position + positionToRotation;

        // Vector3.up와 회전 방향의 외적을 구해서 회전 축으로 한다
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;


        // 90도 + 경사로의 각도(26.1423) 만큼 회전
        float angle = 0;
        while (angle < 116.1423f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }

        // 회전이 116도보다 적거나 많이 될 때 116도로 조정
        transform.RotateAround(point, axis, 116.1423f - angle);

        // 116도 회전 이후 다음 계단까지 거리를 더하여 경사로 길이를 보정
        SlopeDistance.x += 0.1f;
        SlopeDistance.y += 0.5f;

        // 현재 위치에서 경사로 길이를 더하여 목표점을 구함
        Vector3 _slopePoint = transform.position;
        switch (_cubePos)
        {
            case CubePos.Up:
                _slopePoint += new Vector3(0, -SlopeDistance.y, SlopeDistance.x);
                break;
            case CubePos.Down:
                _slopePoint += new Vector3(0, -SlopeDistance.y, -SlopeDistance.x);
                break;
            case CubePos.Right:
                _slopePoint += new Vector3(SlopeDistance.x, -SlopeDistance.y, 0);
                break;
            case CubePos.Left:
                _slopePoint += new Vector3(-SlopeDistance.x, -SlopeDistance.y, 0);
                break;
        }

        // 목표점과 가까워질 때까지 이동
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _slopePoint, _slopeSpeed * Time.deltaTime);
            if (Mathf.Abs(_slopePoint.sqrMagnitude - transform.position.sqrMagnitude) < 0.01f)
                break;
            yield return null;
        }

        // 가장 자연스럽게 회전되는 위치로 회전 기준점을 변경
        point = transform.position + new Vector3(2 * positionToRotation.x * 0.2f, 2 * positionToRotation.y * 0.7f, 2 * positionToRotation.z * 0.2f);

        // 경사로 목표점에 도달시 180도 까지 회전
        angle = 116.1423f;
        while (angle < 180f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }

        // 회전이 180도보다 적거나 많이 될 때 180도로 조정
        transform.RotateAround(point, axis, 180f - angle);

        // CubeCheck를 큐브 위로 이동 후 CubeCheck를 활성화
        _cubeChecker.RePosition(transform.position);
        _cubeCheckerObj.SetActive(true);

        // 경사로 끝에서 position을 반올림해서 보정
        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));

        // 경사로 종료, 이동 가능
        IsRolling = false;
    }

    private void CubeFall()
    {
        // 스탬프 콜라이더들 비활성화
        foreach (BoxCollider box in stampPoints) box.enabled = false;

        // 낙하 중 이동 막기
        IsRolling = true;

        // 물리 활성화
        _rigidbody.isKinematic = false;

        // 콜라이더 축소로 낙하 유도
        transform.GetComponent<BoxCollider>().size = new Vector3(0.9f, 0.9f, 0.9f);
    }

    // CubeFall()에서 물리가 활성화 될 때만 OnCollisionEnter가 실행될 수 있음
    private void OnCollisionEnter(Collision collision)
    {
        // 물리 비활성화
        _rigidbody.isKinematic = true;

        // 콜라이더 원복
        transform.GetComponent<BoxCollider>().size = Vector3.one;

        // CubeCheck를 큐브 위로 이동
        _cubeChecker.RePosition(transform.position);

        // 낙하 이후 position을 반올림해서 보정
        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));

        // 낙하 종료, 이동 가능
        IsRolling = false;

        // 스탬프 콜라이더들 활성화
        foreach (BoxCollider box in stampPoints) box.enabled = true;
    }
    public void FallCheck()
    {
        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    // 큐브의 블로킹을 제거하는 함수
    public void BlockingReset()
    {
        for (int i = 0; i < 4; i++)
        {
            BlockingDir[i] = CubePos.None;
        }
    }
}
