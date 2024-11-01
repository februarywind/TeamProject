using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStage3 : MonoBehaviour
{
    public void LoadStage(int stageNumber)
    {
        string sceneName = "Stage" + stageNumber + " temp";
        SceneManager.LoadScene(sceneName);
    }
}
