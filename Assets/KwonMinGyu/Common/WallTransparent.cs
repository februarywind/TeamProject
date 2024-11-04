using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparent : MonoBehaviour
{
    void Update()
    {
        transform.position = CubeMove.Instance.transform.position;
        transform.LookAt(Camera.main.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 0) return;
        foreach (var item in other.GetComponentsInChildren<Renderer>())
        {
            Material temp = item.GetComponent<Material>();
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 1f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 0) return;
        foreach (var item in other.GetComponentsInChildren<Renderer>())
        {
            Material temp = item.GetComponent<Material>();
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 0f);
        }
    }
}
