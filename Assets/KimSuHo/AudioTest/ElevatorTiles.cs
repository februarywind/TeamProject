using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTiles : MonoBehaviour
{
    private bool isSoundPlaying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CubeStamp") && !isSoundPlaying)
        {
            isSoundPlaying = true; // 효과음이 재생 중임을 표시
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.ElevatorTiles);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CubeStamp") && isSoundPlaying)
        {
            isSoundPlaying = false; // 효과음이 재생 중임을 표시
        }
    }
}
