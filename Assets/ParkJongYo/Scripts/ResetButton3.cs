using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton3 : MonoBehaviour
{
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); // 모든 저장 데이터를 삭제하여 초기화
        PlayerPrefs.Save();      // 변경 사항 저장
        Debug.Log("씬 저장 초기화");

        // 현재 씬을 다시 로드하여 초기 상태로 되돌리기
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
