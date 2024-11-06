using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject _loadScene;
    public string targetSceneName = "Stage2 temp_remodeled"; // 목표 씬 이름
    public int currentStageNumber = 1; // 현재 스테이지 번호 (잠금 해제를 위해 필요)

    private void Update()
    {
        // 아무 키를 눌렀을 때 로딩 씬으로 전환
        if (Input.anyKeyDown)
        {
            Debug.Log("로딩 씬으로 전환 중...");
            StartCoroutine(LoadSceneWithDelay());
        }
    }

    private IEnumerator LoadSceneWithDelay()
    {
        // 로딩 씬으로 전환
        _loadScene.SetActive(true);

        // 3초 대기
        Debug.Log("3초 대기 중...");
        yield return new WaitForSeconds(3f);

        // 다음 스테이지 잠금 해제
        UnlockNextStage();

        // 목표 씬으로 전환
        Debug.Log($"목표 씬 '{targetSceneName}'로 전환 중...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);

        // 목표 씬이 로드될 때까지 대기
        while (!asyncLoad.isDone)
        {
            Debug.Log($"스테이지 '{targetSceneName}' 로딩 진행 중: {asyncLoad.progress * 100}%");
            yield return null; // 매 프레임 대기
        }

        Debug.Log($"목표 씬 '{targetSceneName}' 로드 완료");
    }

    private void UnlockNextStage()
    {
        // StageManager 찾기
        StageManager stageManager = FindObjectOfType<StageManager>();
        if (stageManager != null)
        {
            // 현재 스테이지를 클리어하고 다음 스테이지 잠금 해제
            stageManager.UnlockNextStage(currentStageNumber);
            Debug.Log($"스테이지 {currentStageNumber + 1} 잠금 해제");
        }
        else
        {
            Debug.LogWarning("StageManager를 찾을 수 없습니다.");
        }
    }
}