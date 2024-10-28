using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeChecker4 : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CubeMove4 move;
    public bool IsGround()
    {
        RaycastHit hit;
        bool _IsGround = Physics.Raycast(transform.position, -transform.up, out hit, 3, layerMask);
        // Debug.Log(Vector3.Angle(Vector3.up, hit.normal)); // 경사로 각도 확인
        return _IsGround;
    }
}
