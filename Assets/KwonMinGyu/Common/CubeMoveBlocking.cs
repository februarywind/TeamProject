using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMoveBlocking : MonoBehaviour
{
    // 이동을 막을 방향
    [SerializeField] private CubePos[] _blockingDir;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CubeMove cubeMove = other.GetComponent<CubeMove>();
        cubeMove.IsBlockingForward = true;
        cubeMove.BlockingDir = _blockingDir;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CubeMove cubeMove = other.GetComponent<CubeMove>();
        cubeMove.IsBlockingForward = false;
    }
}
