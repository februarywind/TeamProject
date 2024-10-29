using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator4 : MonoBehaviour
{
    [SerializeField] private float _targetFloor; // 엘리베이터가 이동할 목표 위치
    [SerializeField] private float _speed = 2f; // 이동 속도

    private Vector3 _targetPos;
    private Vector3 _startPos;

    private Coroutine _elevatorCoroutine;

    private void Start()
    {
        _startPos = transform.position;
        _targetPos = transform.position + Vector3.up * _targetFloor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _elevatorCoroutine = StartCoroutine(ElevatorEnter(other.GetComponent<CubeMove4>()));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _elevatorCoroutine = StartCoroutine(ElevatorExit());
    }

    IEnumerator ElevatorEnter(CubeMove4 _cube)
    {
        if (_elevatorCoroutine != null) 
            StopCoroutine( _elevatorCoroutine);
        while (true)
        {
            if (!_cube.IsRolling)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
                _cube.gameObject.transform.position = transform.position;
                if (Mathf.Abs(_targetPos.sqrMagnitude - transform.position.sqrMagnitude) < 0.01f)
                    break;
            }
            yield return null;
        }
    }
    IEnumerator ElevatorExit()
    {
        if (_elevatorCoroutine != null)
            StopCoroutine(_elevatorCoroutine);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, _speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.sqrMagnitude - _startPos.sqrMagnitude) < 0.01f)
                break;
            yield return null;
        }
        yield return null;
    }
}
