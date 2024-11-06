using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider; // UI 슬라이더
    public TextMeshProUGUI sensitivityValueText; // 감도 값을 표시할 TextMeshPro 텍스트

    private void Start()
    {
        // 슬라이더의 최소값과 최대값을 설정
        sensitivitySlider.minValue = 0f;
        sensitivitySlider.maxValue = 100f;

        // PlayerPrefs에서 기본 감도 설정값을 불러옵니다. 없으면 50으로 설정
        float defaultSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 50f);
        sensitivitySlider.value = defaultSensitivity;

        // 초기 텍스트 값 설정 (정수로 표시)
        UpdateSensitivityValueText(defaultSensitivity);

        // 슬라이더 값 변경 이벤트 등록
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChange);
    }

    // 슬라이더 값이 변경되면 호출되는 메서드
    private void OnSensitivityChange(float value)
    {
        // 변경된 감도를 PlayerPrefs에 저장
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();

        // 텍스트 업데이트
        UpdateSensitivityValueText(value);
    }

    // 감도 값 텍스트 업데이트
    private void UpdateSensitivityValueText(float value)
    {
        // 소수점 없이 정수로만 표시
        sensitivityValueText.text = Mathf.RoundToInt(value).ToString();
    }
}
