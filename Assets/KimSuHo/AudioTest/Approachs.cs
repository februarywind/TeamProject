using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approachs : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Approach);
    }
}
