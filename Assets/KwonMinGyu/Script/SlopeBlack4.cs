using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlopeBlack4 : MonoBehaviour
{
    [SerializeField] private CubePos4 _slopeDir;

    // x값은 경사로의 길이, y값은 경사로의 높이를 입력
    [SerializeField] private Vector2 _slopeDistance;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CubeMove4 cubeMove4 = other.GetComponent<CubeMove4>();
        cubeMove4.IsSlopeForward = true;
        cubeMove4.SlopeDir = _slopeDir;
        cubeMove4.SlopeDistance = _slopeDistance - new Vector2(1, 1);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CubeMove4 cubeMove4 = other.GetComponent<CubeMove4>();
        cubeMove4.IsSlopeForward = false;
    }
}
