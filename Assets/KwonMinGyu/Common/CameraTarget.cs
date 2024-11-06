using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform _player;
    [SerializeField] float _speed;
    void Start()
    {
        _player = CubeMove.Instance.transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _player.position, _speed * Time.deltaTime);
    }
}
