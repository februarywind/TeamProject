using System;
using System.Collections;
using UnityEngine;
enum CubePos
{
    Up, Down, Right, Left
}
public class CubeMove4 : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private CameraMove4 _cameraMove;
    [SerializeField] private CubeChecker4 _cubeChecker;
    [SerializeField] private BoxCollider _cubeUpCheck;
    [SerializeField] private Rigidbody _rigidbody;

    CameraPos _cameraPos;
    CubePos _cubePos;

    Vector3[] _moveDir = new Vector3[4];
    Bounds bound;

    bool IsRolling;
    
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
        if (!IsRolling)
            StartCoroutine(Roll(_cubePos));
    }

    IEnumerator Roll(CubePos cubePos)
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
