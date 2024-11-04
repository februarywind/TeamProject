using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Bgm : MonoBehaviour
{
    [SerializeField] float bgmVolume = 0.2f;

    void Start()
    {
        AudioManager.Instance.PlayBgm(3);
        AudioManager.Instance.SetBgmVolume(bgmVolume);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V))
    //    {
    //        bgmVolume += 0.1f;
    //        bgmVolume = Mathf.Clamp(bgmVolume, 0.0f, 1.0f);
    //        AudioManager.Instance.SetBgmVolume(bgmVolume);
    //    }
    //}
}
