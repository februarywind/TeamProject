using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] int _bgmNum;
    private void Start()
    {
        AudioManager.Instance.PlayBgm(_bgmNum);
    }
    public void PlaySFX(int sfx)
    {
        AudioManager.Instance.PlaySfx((AudioManager.Sfx)sfx);
    }
}
