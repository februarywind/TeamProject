using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Button stage1Button;
    public Button stage2Button;
    public Button stage3Button;
    public Button stage4Button;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitializeStages();
        CheckStageUnlocks();

        if (stage1Button != null)
            stage1Button.onClick.AddListener(LoadStage1);
        if (stage2Button != null)
            stage2Button.onClick.AddListener(LoadStage2);
        if (stage3Button != null)
            stage3Button.onClick.AddListener(LoadStage3);
        if (stage4Button != null)
            stage4Button.onClick.AddListener(LoadStage4);
    }

    void InitializeStages()
    {
        if (!PlayerPrefs.HasKey("Stage1Unlocked"))
        {
            PlayerPrefs.SetInt("Stage1Unlocked", 1);
            for (int i = 2; i <= 4; i++)
            {
                PlayerPrefs.SetInt($"Stage{i}Unlocked", 0);
            }
            PlayerPrefs.Save();
        }
    }

    void CheckStageUnlocks()
    {
        if (stage1Button != null)
            stage1Button.interactable = PlayerPrefs.GetInt("Stage1Unlocked") == 1;
        if (stage2Button != null)
            stage2Button.interactable = PlayerPrefs.GetInt("Stage2Unlocked") == 1;
        if (stage3Button != null)
            stage3Button.interactable = PlayerPrefs.GetInt("Stage3Unlocked") == 1;
        if (stage4Button != null)
            stage4Button.interactable = PlayerPrefs.GetInt("Stage4Unlocked") == 1;
    }

    public void UnlockNextStage(int currentStage)
    {
        if (currentStage >= 1 && currentStage < 4)
        {
            // 현재 스테이지 클리어 시 다음 스테이지 잠금 해제
            PlayerPrefs.SetInt($"Stage{currentStage + 1}Unlocked", 1);
            PlayerPrefs.Save();
            CheckStageUnlocks();

            // 다음 스테이지 로딩 호출
            LoadNextStageWithLoading(currentStage);
        }
    }

    public void LoadNextStageWithLoading(int nextStage)
    {
        // 다음 스테이지 번호에 맞는 로딩 씬과 타겟 스테이지 씬 설정
        string targetStageName = $"Stage{nextStage} temp";
        string loadingSceneName = $"LoadingScene{nextStage}";

        LoadStageWithLoading(targetStageName, loadingSceneName);
    }

    public void LoadStage1() => LoadStageWithLoading("Stage1 temp_remodeled", "LoadingScene1");
    public void LoadStage2() => LoadStageWithLoading("Stage2 temp_remodeled", "LoadingScene2");
    public void LoadStage3() => LoadStageWithLoading("Stage3 temp", "LoadingScene3");
    public void LoadStage4() => LoadStageWithLoading("Stage4 temp", "LoadingScene4");

    public void LoadStageWithLoading(string targetStageName, string loadingSceneName)
    {
        Debug.Log($"타겟 스테이지: {targetStageName}, 로딩 씬: {loadingSceneName}");
        StartCoroutine(LoadStageWithLoadingScreen(loadingSceneName, targetStageName));
    }

    public IEnumerator LoadStageWithLoadingScreen(string loadingSceneName, string targetStageName)
    {
        Debug.Log($"로딩 씬 '{loadingSceneName}' 로드 중...");
        AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync(loadingSceneName);
        yield return new WaitUntil(() => loadLoadingScene.isDone);

        Debug.Log("로딩 화면을 3초 동안 표시합니다...");
        yield return new WaitForSeconds(3f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetStageName);
        yield return new WaitUntil(() => asyncLoad.isDone);

        Debug.Log($"목표 스테이지 '{targetStageName}' 로드 완료");
    }
}
