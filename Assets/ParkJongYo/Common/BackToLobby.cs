using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobby : MonoBehaviour
{
    private void Update()
    {
        // ESC 키를 눌렀을 때 로비 씬으로 전환
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.root.gameObject.SetActive(false);
        }
    }
}
