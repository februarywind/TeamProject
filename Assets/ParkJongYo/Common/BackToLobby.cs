using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobby : MonoBehaviour
{
    [SerializeField] private string lobbySceneName = "110_LobbyScene";

    private void Update()
    {
        // ESC 키를 눌렀을 때 로비 씬으로 전환
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadLobbyScene();
        }
    }

    // 로비 씬으로 전환하는 메서드
    private void LoadLobbyScene()
    {
        SceneManager.LoadScene(lobbySceneName);
    }
}
