using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 0) return;
        foreach (var item in other.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 0) return;
        foreach (var item in other.GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = true;
        }
    }
}
