using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStartScene : MonoBehaviour
{
    [SerializeField] private string startSceneName = "100_StartScene";
    private void Update()
    {
        // ESC 키를 눌렀을 때 로비 씬으로 전환
        if (Input.anyKeyDown)
        {
            // 마우스 커서를 다시 활성화
            Cursor.lockState = CursorLockMode.None; // 커서를 잠금 해제
            Cursor.visible = true; // 커서를 보이도록 설정

            SceneManager.LoadScene(startSceneName);
        }
    }
}
