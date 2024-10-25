using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeChecker4 : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    public bool IsGround()
    {
        return Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 3, layerMask);
    }
}
