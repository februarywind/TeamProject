using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMoveBlocking4 : MonoBehaviour
{
    // 이동을 막을 방향
    [SerializeField] private CubePos4[] _blockingDir;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        CubeMove4 cubeMove4 = other.GetComponent<CubeMove4>();
        cubeMove4.IsBlockingForward = true;
        cubeMove4.BlockingDir = _blockingDir;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        CubeMove4 cubeMove4 = other.GetComponent<CubeMove4>();
        cubeMove4.IsBlockingForward = false;
    }
}
