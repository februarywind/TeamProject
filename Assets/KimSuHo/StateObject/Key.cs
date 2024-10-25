using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            if (other.gameObject != null)
            {
                Debug.Log("±Â±Â");
            }
        }
    }
}
