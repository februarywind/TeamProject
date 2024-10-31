using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Goal : MonoBehaviour
{
    [SerializeField] float WaitTime = 3f;
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그가 "Player"인 경우에만 처리
        if (other.CompareTag("Player"))
        {
            Debug.Log("골인점 도착");
            // 코루틴 시작
            StartCoroutine(LoadStageWithDelay());
        }
    }

    private IEnumerator LoadStageWithDelay()
    {
        Debug.Log("3초 후 Stage2로 전환합니다.");
        // WaitTime만큼 대기
        yield return new WaitForSeconds(WaitTime);
        // Stage2로 전환
        SceneManager.LoadScene("Stage2");
    }
}
