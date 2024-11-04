using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparent : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.transform.gameObject.layer)) == 0) return;
        foreach (var item in other.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.transform.gameObject.layer)) == 0) return;
        foreach (var item in other.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = true;
        }
    }
}
