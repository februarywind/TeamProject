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
        return _IsGround;
    }
}
