using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public enum CubePos4
{
    Up, Down, Right, Left
}
public class CubeMove4 : MonoBehaviour
{
    // 큐브 회전속도
    [SerializeField] private float _rotationSpeed;

    // 큐브 윗면 체크 및 바닥 체크를 위한 CubeChecker 스크립트
    [SerializeField] private CubeChecker4 _cubeChecker;
    [SerializeField] private BoxCollider _cubeUpCheck;

    // 큐브 낙하를 위한 Rigidbody
    [SerializeField] private Rigidbody _rigidbody;

    // 카메라 방향을 구하기 위한 CameraMove 스크립트
    [SerializeField] private CameraMove4 _cameraMove;
    CameraPos4 _cameraPos;

    // 카메라 뱡향에 따른 큐브 회전을 위한 필드
    private Vector3[] _moveDir = new Vector3[4];
    private CubePos4 _cubePos;
    private Bounds bound;

    // true일 때 플레이어의 입력이 막힌다.
    private bool _IsRolling;

    // 경사로 이동속도
    [SerializeField] private float _slopeSpeed;

    // 경사로 확인 필드 SlopeBlack와 Trigger될 때 자동 입력됨
    public bool IsSlopeForward;
    public CubePos4 SlopeDir;
    public Vector2 SlopeDistance;

    // 특정 칸에서 큐브 이동을 막기 위한 필드
    public bool IsBlockingForward;
    public CubePos4[] BlockingDir;

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
        float _HMove = Input.GetAxisRaw("Horizontal");
        float _VMove = Input.GetAxisRaw("Vertical");

        if (_HMove == 0 && _VMove == 0) return;

        // 처음 큐브 위치를 기준으로 카메라 방향을 구함
        _cameraPos = _cameraMove.CameraPosition();

        // Move Forward
        if (_VMove == 1)
        {
            // 카메라 방향에 따라 회전 방향을 변경
            switch (_cameraPos)
            {
                case CameraPos4.Up:
                    _cubePos = CubePos4.Down;
                    break;
                case CameraPos4.Down:
                    _cubePos = CubePos4.Up;
                    break;
                case CameraPos4.Right:
                    _cubePos = CubePos4.Left;
                    break;
                case CameraPos4.Left:
                    _cubePos = CubePos4.Right;
                    break;
            }
        }

        // Move Backwards
        else if (_VMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos4.Up:
                    _cubePos = CubePos4.Up;
                    break;
                case CameraPos4.Down:
                    _cubePos = CubePos4.Down;
                    break;
                case CameraPos4.Right:
                    _cubePos = CubePos4.Right;
                    break;
                case CameraPos4.Left:
                    _cubePos = CubePos4.Left;
                    break;
            }
        }

        // Move Right
        else if (_HMove == 1)
        {
            switch (_cameraPos)
            {
                case CameraPos4.Up:
                    _cubePos = CubePos4.Left;
                    break;
                case CameraPos4.Down:
                    _cubePos = CubePos4.Right;
                    break;
                case CameraPos4.Right:
                    _cubePos = CubePos4.Up;
                    break;
                case CameraPos4.Left:
                    _cubePos = CubePos4.Down;
                    break;
            }
        }

        // Move Left
        else if (_HMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos4.Up:
                    _cubePos = CubePos4.Right;
                    break;
                case CameraPos4.Down:
                    _cubePos = CubePos4.Left;
                    break;
                case CameraPos4.Right:
                    _cubePos = CubePos4.Down;
                    break;
                case CameraPos4.Left:
                    _cubePos = CubePos4.Up;
                    break;
            }
        }

        if (!_IsRolling)
        {
            // 이동 못하는 방향으로 이동 시 리턴
            if (IsBlockingForward && BlockingDir.Contains(_cubePos))
                return;

            // 경사로 앞에서 경사로 쪽으로 이동시 경사로 이동 코루틴 실행
            if (IsSlopeForward && SlopeDir == _cubePos)
            {
                StartCoroutine(SlopeRoll(_cubePos));
                return;
            }

            StartCoroutine(Roll(_cubePos));
        }
    }

    IEnumerator Roll(CubePos4 cubePos)
    {
        Vector3 positionToRotation = _moveDir[(int)cubePos];

        _IsRolling = true;
        _cubeUpCheck.enabled = false; // 이동 중 다른 콜라이더와 접촉하지 않도록 _cubeUpCheck 비활성화

        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;
        Debug.Log(axis);

        while (angle < 90f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed); // (기준점, 방향, 회전값)으로 회전
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 90 - angle); // 회전이 90도보다 적거나 많이 될 때 90도로 조정

        _IsRolling = false;

        _cubeUpCheck.transform.position = transform.position + Vector3.up; // CubeCheck를 큐브 위로 이동
        _cubeUpCheck.enabled = true;

        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    IEnumerator SlopeRoll(CubePos4 cubePos)
    {
        Vector3 positionToRotation = _moveDir[(int)cubePos];

        _IsRolling = true;
        _cubeUpCheck.enabled = false;

        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        while (angle < 116.1423f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 116.1423f - angle);

        // 경사로 끝의 목표 저장
        Vector3 _slopePoint = transform.position;
        SlopeDistance.x += 0.1f;
        SlopeDistance.y += 0.5f;
        switch (_cubePos)
        {
            case CubePos4.Up:
                _slopePoint += new Vector3(0, -SlopeDistance.y, SlopeDistance.x);
                break;
            case CubePos4.Down:
                _slopePoint += new Vector3(0, -SlopeDistance.y, -SlopeDistance.x);
                break;
            case CubePos4.Right:
                _slopePoint += new Vector3(SlopeDistance.x, -SlopeDistance.y, 0);
                break;
            case CubePos4.Left:
                _slopePoint += new Vector3(-SlopeDistance.x, -SlopeDistance.y, 0);
                break;
        }
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _slopePoint, _slopeSpeed * Time.deltaTime);
            if (_slopePoint.sqrMagnitude - transform.position.sqrMagnitude < 0.01f)
                break;
            yield return null;
        }
        angle = 116.1423f;
        point = transform.position + (-Vector3.up * 0.5f * MathF.Sqrt(2)) + (-Vector3.forward * 0.25f);
        while (angle < 180f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }
        transform.RotateAround(point, axis, 180f - angle);
        _IsRolling = false;

        _cubeUpCheck.transform.position = transform.position + Vector3.up;
        _cubeUpCheck.enabled = true;

        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));
    }

    private void CubeFall()
    {
        _IsRolling = true; // 낙하 중 회전 막기
        _rigidbody.isKinematic = false; // 물리를 활성화
        transform.GetComponent<BoxCollider>().size = new Vector3(0.9f, 0.9f, 0.9f); // 콜라이더 축소로 낙하 유도
    }

    // CubeFall()에서 물리가 활성화 될 때만 OnCollisionEnter가 실행될 수 있음
    private void OnCollisionEnter(Collision collision)
    {
        _IsRolling = false;
        _rigidbody.isKinematic = true;

        transform.GetComponent<BoxCollider>().size = Vector3.one; // 콜라이더 원복

        _cubeUpCheck.transform.position = transform.position + Vector3.up; // _cubeUpCheck 큐브 위로 이동

        // 낙하 이후 좌표값 보정
        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));
    }
}
