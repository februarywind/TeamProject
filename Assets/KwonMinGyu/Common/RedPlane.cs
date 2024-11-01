using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlane : MonoBehaviour
{
    void Update()
    {
        if (CubeMove.Instance.IsRolling) return;
        transform.position = CubeMove.Instance.transform.position + Vector3.down * 5f;
    }
}
