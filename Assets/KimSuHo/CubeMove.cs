using System.Collections;
using UnityEngine;
enum CubePos
{
    Up, Down, Right, Left
}
public class CubeMove : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private BoxCollider _cubeUpCheck;
    [SerializeField] private CameraMove _cameraMove;
    CameraPos _cameraPos;
    CubePos _cubePos;
    bool IsRolling;

    Bounds bound;
    Vector3[] _moveDir = new Vector3[4];
    Vector3 left, right, up, down;

    void Start()
    {
        bound = GetComponent<BoxCollider>().bounds;
        _moveDir[0] = new Vector3(0, -bound.size.y / 2, bound.size.z / 2);
        _moveDir[1] = new Vector3(0, -bound.size.y / 2, -bound.size.z / 2);
        _moveDir[2] = new Vector3(bound.size.x / 2, -bound.size.y / 2, 0);
        _moveDir[3] = new Vector3(-bound.size.x / 2, -bound.size.y / 2, 0);
    }

    void Update()
    {
        _cameraPos = _cameraMove.CameraPosition();
        // Move Forward
        float _HMove = Input.GetAxisRaw("Horizontal");
        float _VMove = Input.GetAxisRaw("Vertical");

        if (_HMove == 0 && _VMove == 0) return;

        if (_VMove == 1)
        {
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
        switch (cubePos)
        {
            case CubePos.Up:
                _cubeUpCheck.transform.position += new Vector3(0, 0, 1);
                break;
            case CubePos.Down:
                _cubeUpCheck.transform.position += new Vector3(0, 0, -1);
                break;
            case CubePos.Right:
                _cubeUpCheck.transform.position += new Vector3(1, 0, 0);
                break;
            case CubePos.Left:
                _cubeUpCheck.transform.position += new Vector3(-1, 0, 0);
                break;
        }
        IsRolling = true;
        _cubeUpCheck.enabled = false;
        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        while (angle < 90f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 90 - angle);
        IsRolling = false;
        _cubeUpCheck.enabled = true;
    }
}
