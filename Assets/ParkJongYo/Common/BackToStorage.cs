using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStorage : MonoBehaviour
{
    [SerializeField] private string StorageSceneName = "111_StorageScene";

    private void Update()
    {
        // ESC 키를 눌렀을 때 로비 씬으로 전환
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadStorageScene();
        }
    }

    // 로비 씬으로 전환하는 메서드
    private void LoadStorageScene()
    {
        SceneManager.LoadScene(StorageSceneName);
    }
}
