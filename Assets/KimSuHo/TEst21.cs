using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst21 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.GetKey();
            }
        }
    }
}
