using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolues : MonoBehaviour
{
    public Slider bgmSlider; // BGM 슬라이더
    public Slider sfxSlider; // SFX 슬라이더
    public Slider mouseSlider; // 마우스 감도 슬라이더

    void Start()
    {
        // 슬라이더의 현재 값을 AudioManager의 볼륨으로 설정
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;

        // 슬라이더 값 변경 시 메서드 연결
        // onValueChanged : float 타입 슬라이더의 값이 변경될 때 호출된다.
        // AddListener : 슬라이더의 값이 변경 될 때 호출할 메서드를 지정 해준다.
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        mouseSlider.onValueChanged.AddListener(Camera.main.GetComponent<CameraMove>().SetMouseSensitivity);
    }

    // BGM 볼륨 설정 메서드
    public void SetBgmVolume(float value)
    {
        AudioManager.Instance.SetBgmVolume(value);
    }

    // SFX 볼륨 설정 메서드
    public void SetSfxVolume(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }
}
