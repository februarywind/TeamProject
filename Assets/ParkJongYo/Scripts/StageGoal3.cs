using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageGoal3 : MonoBehaviour
{
    [SerializeField] int currentStage;
    [SerializeField] string nextStageName;
    [SerializeField] float waitTime = 3f; // 전환 대기 시간

    private StageManager3 stageManager;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager3>(); // StageManager 찾기
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
        stageManager.UnlockNextStage(currentStage);
        // 다음 스테이지로 전환
        SceneManager.LoadScene(nextStageName);
    }
}
