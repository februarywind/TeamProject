using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageGoal : MonoBehaviour
{
    [SerializeField] string clearSceneName; // 각 스테이지의 클리어 씬 이름
    [SerializeField] float waitTime = 1f; // 전환 대기 시간

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("골인점 도착");
            StartCoroutine(LoadClearSceneWithDelay());
        }
    }

    private IEnumerator LoadClearSceneWithDelay()
    {
        yield return new WaitForSeconds(waitTime);

        // 클리어 씬으로 전환
        SceneManager.LoadScene(clearSceneName);
    }
}
