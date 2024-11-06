using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour
{
    public void LoadStage(int stageNumber)
    {
        string sceneName = "Stage" + stageNumber + " temp" + "_remodeled";
        SceneManager.LoadScene(sceneName);
    }
}
