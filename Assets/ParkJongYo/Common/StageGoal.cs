using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageGoal : MonoBehaviour
{
    [SerializeField] int currentStage; // 현재 스테이지
    [SerializeField] string nextStageName; // 다음 스테이지 이름
    [SerializeField] float waitTime = 3f; // 전환 대기 시간

    private StageManager stageManager;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>(); // StageManager 찾기
        if (stageManager == null)
        {
            Debug.LogError("StageManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("골인점 도착");
            StartCoroutine(LoadNextStageWithDelay());
        }
    }

    private IEnumerator LoadNextStageWithDelay()
    {
        Debug.Log($"{waitTime}초 후 {nextStageName}로 전환합니다.");
        yield return new WaitForSeconds(waitTime);

        // 다음 스테이지 잠금 해제
        if (stageManager != null)
        {
            stageManager.UnlockNextStage(currentStage);
            // 로딩 씬을 거쳐 다음 스테이지로 전환
            StartCoroutine(stageManager.LoadStageWithLoadingScreen($"LoadingScene{currentStage}", nextStageName));
        }
    }
}
