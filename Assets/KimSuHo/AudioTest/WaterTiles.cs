using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTiles : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CubeStamp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.WaterTile);
        }
    }
}
