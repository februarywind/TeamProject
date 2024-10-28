using System;
using System.Collections;
using UnityEngine;
public enum CubePos4
{
    Up, Down, Right, Left
}
public class CubeMove4 : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _SlopeSpeed;
    [SerializeField] private CameraMove4 _cameraMove;
    [SerializeField] private CubeChecker4 _cubeChecker;
    [SerializeField] private BoxCollider _cubeUpCheck;
    [SerializeField] private Rigidbody _rigidbody;

    CameraPos4 _cameraPos;
    CubePos4 _cubePos;

    Vector3[] _moveDir = new Vector3[4];
    Bounds bound;

    bool IsRolling;

    public bool slopeForward;
    public CubePos4 slopeDir;

    void Start()
    {
        // bound.size값으로 콜라이더의 월드기준 size를 구할 수 있음
        // 아래 값들은 RotateAround의 회전 기준점이 됨
        bound = GetComponent<BoxCollider>().bounds;
        _moveDir[0] = new Vector3(0, -bound.size.y / 2, bound.size.z / 2);
        _moveDir[1] = new Vector3(0, -bound.size.y / 2, -bound.size.z / 2);
        _moveDir[2] = new Vector3(bound.size.x / 2, -bound.size.y / 2, 0);
        _moveDir[3] = new Vector3(-bound.size.x / 2, -bound.size.y / 2, 0);

        _rigidbody = GetComponent<Rigidbody>();

        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    void Update()
    {
        _cameraPos = _cameraMove.CameraPosition();
        float _HMove = Input.GetAxisRaw("Horizontal");
        float _VMove = Input.GetAxisRaw("Vertical");

        if (_HMove == 0 && _VMove == 0) return;


        // Move Forward
        if (_VMove == 1)
        {
            // CameraPosition()로 카메라 방향을 구해 회전 방향을 변경
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
        if (!IsRolling)
        {
            // 경사로 앞에서 경사로 쪽으로 이동시 경사로 이동 코루틴 실행
            if (slopeForward && slopeDir == _cubePos)
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

        IsRolling = true;
        _cubeUpCheck.enabled = false; // 이동 중 다른 콜라이더와 접촉하지 않도록 _cubeUpCheck 비활성화

        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        while (angle < 90f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed); // (기준점, 방향, 회전값)으로 회전
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 90 - angle); // 회전이 90도보다 적거나 많이 될 때 90도로 조정

        IsRolling = false;

        _cubeUpCheck.transform.position = transform.position + Vector3.up; // _cubeUpCheck 큐브 위로 이동
        _cubeUpCheck.enabled = true;

        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    IEnumerator SlopeRoll(CubePos4 cubePos)
    {
        Vector3 positionToRotation = _moveDir[(int)cubePos];

        IsRolling = true;
        _cubeUpCheck.enabled = false; // 이동 중 다른 콜라이더와 접촉하지 않도록 _cubeUpCheck 비활성화

        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        while (angle < 134f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 134f - angle);

        // 경사로 끝의 목표 저장
        Vector3 _slopePoint = new();
        switch (_cubePos)
        {
            case CubePos4.Up:
                _slopePoint = new Vector3(0, -3.3f, 3.3f);
                break;
            case CubePos4.Down:
                _slopePoint = new Vector3(0, -3.3f, -3.3f);
                break;
            case CubePos4.Right:
                _slopePoint = new Vector3(3.3f, -3.3f, 0);
                break;
            case CubePos4.Left:
                _slopePoint = new Vector3(-3.3f, -3.3f, 0);
                break;
        }
        Vector3 TPos = transform.position + _slopePoint;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TPos, _SlopeSpeed * Time.deltaTime);
            if (TPos.sqrMagnitude - transform.position.sqrMagnitude < 0.01f)
                break;
            yield return null;
        }
        angle = 134f;
        point = transform.position + (-Vector3.up * 0.5f * MathF.Sqrt(2));
        while (angle < 180f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }
        transform.RotateAround(point, axis, 180f - angle);
        IsRolling = false;

        _cubeUpCheck.transform.position = transform.position + Vector3.up;
        _cubeUpCheck.enabled = true;

        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));
    }

    private void CubeFall()
    {
        IsRolling = true; // 떨어지는 중 회전 막기
        _rigidbody.isKinematic = false; // 물리를 활성화
        transform.GetComponent<BoxCollider>().size = new Vector3(0.9f, 0.9f, 0.9f); // 콜라이더 축소로 낙하 유도
    }

    // CubeFall()에서 물리가 활성화 될 때만 OnCollisionEnter가 실행될 수 있음
    private void OnCollisionEnter(Collision collision)
    {
        IsRolling = false;
        _rigidbody.isKinematic = true;

        transform.GetComponent<BoxCollider>().size = Vector3.one; // 콜라이더 원복

        _cubeUpCheck.transform.position = transform.position + Vector3.up; // _cubeUpCheck 큐브 위로 이동

        // 낙하 이후 좌표값 보정
        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));
    }
}
