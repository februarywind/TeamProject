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
            SceneManager.LoadScene(startSceneName);
        }
    }
}
