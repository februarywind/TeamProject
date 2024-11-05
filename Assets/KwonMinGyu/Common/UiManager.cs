using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject _uiObj;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uiObj.SetActive(!_uiObj.activeSelf);
            Cursor.lockState = _uiObj.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = (_uiObj.activeSelf) ? 0f : 1f;
        }
    }

    public void SliderUpdate(Slider _bgmSlider)
    {
        Debug.Log(_bgmSlider.value);
    }
}
