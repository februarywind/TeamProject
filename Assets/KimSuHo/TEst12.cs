using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst12 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.GetRed();
            }
        }
    }
}
