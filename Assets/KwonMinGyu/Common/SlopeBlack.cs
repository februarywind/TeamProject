using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlopeBlack : MonoBehaviour
{
    [SerializeField] private CubePos _slopeDir;

    // x값은 경사로의 길이, y값은 경사로의 높이를 입력
    [SerializeField] private Vector2 _slopeDistance;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CubeMove cubeMove = other.GetComponent<CubeMove>();
        cubeMove.IsSlopeForward = true;
        cubeMove.SlopeDir = _slopeDir;
        cubeMove.SlopeDistance = _slopeDistance - new Vector2(1, 1);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CubeMove cubeMove = other.GetComponent<CubeMove>();
        cubeMove.IsSlopeForward = false;
    }
}
