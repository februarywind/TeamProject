using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private string lobbySceneName = "110_LobbyScene";

    private void Update()
    {
        // 아무 키나 눌렀을 때 로비 씬으로 전환
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(lobbySceneName);
        }
    }
}
