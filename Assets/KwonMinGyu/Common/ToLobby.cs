using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLobby : MonoBehaviour
{
    [SerializeField] float waitTime = 1f; // 전환 대기 시간
    public void Temp()
    {
        // 메뉴를 열면서 바뀐 시간을 원복
        Time.timeScale = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadClearSceneWithDelay());
        }
    }

    private IEnumerator LoadClearSceneWithDelay()
    {
        yield return new WaitForSeconds(waitTime);

        // 로비 씬으로 전환
        SceneManager.LoadScene("110_LobbyScene");
    }
}
