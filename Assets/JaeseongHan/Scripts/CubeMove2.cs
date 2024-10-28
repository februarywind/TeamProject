using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CubePos2
{
    Up, Down, Right, Left
}

public class CubeMove2 : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private BoxCollider _cubeUpCheck;
    [SerializeField] private CameraMove2 _cameraMove;
    CameraPos2 _cameraPos;
    CubePos2 _cubePos;
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
                case CameraPos2.Up:
                    _cubePos = CubePos2.Down;
                    break;
                case CameraPos2.Down:
                    _cubePos = CubePos2.Up;
                    break;
                case CameraPos2.Right:
                    _cubePos = CubePos2.Left;
                    break;
                case CameraPos2.Left:
                    _cubePos = CubePos2.Right;
                    break;
            }
        }

        // Move Backwards
        else if (_VMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos2.Up:
                    _cubePos = CubePos2.Up;
                    break;
                case CameraPos2.Down:
                    _cubePos = CubePos2.Down;
                    break;
                case CameraPos2.Right:
                    _cubePos = CubePos2.Right;
                    break;
                case CameraPos2.Left:
                    _cubePos = CubePos2.Left;
                    break;
            }
        }

        // Move Right
        else if (_HMove == 1)
        {
            switch (_cameraPos)
            {
                case CameraPos2.Up:
                    _cubePos = CubePos2.Left;
                    break;
                case CameraPos2.Down:
                    _cubePos = CubePos2.Right;
                    break;
                case CameraPos2.Right:
                    _cubePos = CubePos2.Up;
                    break;
                case CameraPos2.Left:
                    _cubePos = CubePos2.Down;
                    break;
            }
        }

        // Move Left
        else if (_HMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos2.Up:
                    _cubePos = CubePos2.Right;
                    break;
                case CameraPos2.Down:
                    _cubePos = CubePos2.Left;
                    break;
                case CameraPos2.Right:
                    _cubePos = CubePos2.Down;
                    break;
                case CameraPos2.Left:
                    _cubePos = CubePos2.Up;
                    break;
            }
        }
        if (!IsRolling)
            StartCoroutine(Roll(_cubePos));
    }

    IEnumerator Roll(CubePos2 cubePos)
    {
        Vector3 positionToRotation = _moveDir[(int)cubePos];
        switch (cubePos)
        {
            case CubePos2.Up:
                _cubeUpCheck.transform.position += new Vector3(0, 0, 1);
                break;
            case CubePos2.Down:
                _cubeUpCheck.transform.position += new Vector3(0, 0, -1);
                break;
            case CubePos2.Right:
                _cubeUpCheck.transform.position += new Vector3(1, 0, 0);
                break;
            case CubePos2.Left:
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
