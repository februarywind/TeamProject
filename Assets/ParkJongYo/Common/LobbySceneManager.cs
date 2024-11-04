using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
    [SerializeField] private string storageSceneName = "111_StorageScene";
    [SerializeField] private string settingSceneName = "113_SettingScene";

    // Play 버튼 클릭 시 호출될 메서드
    public void LoadStorageScene()
    {
        SceneManager.LoadScene(storageSceneName);
    }

    // Setting 버튼 클릭 시 호출될 메서드
    public void LoadSettingScene()
    {
        SceneManager.LoadScene(settingSceneName);
    }
    public void QuitGame()
    {
        // 에디터에서 실행 중일 때는 플레이 모드 중지
        UnityEditor.EditorApplication.isPlaying = false;
        // 빌드된 게임에서 종료
        Application.Quit();
    }
}
