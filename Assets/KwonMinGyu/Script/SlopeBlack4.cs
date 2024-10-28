using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlopeBlack4 : MonoBehaviour
{
    [SerializeField] private CubePos4 slopeDir;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        CubeMove4 cubeMove4 = other.GetComponent<CubeMove4>();
        cubeMove4.slopeForward = true;
        cubeMove4.slopeDir = slopeDir;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        CubeMove4 cubeMove4 = other.GetComponent<CubeMove4>();
        cubeMove4.slopeForward = false;
    }
}
