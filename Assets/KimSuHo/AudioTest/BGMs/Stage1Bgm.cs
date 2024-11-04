using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Bgm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBgm(1);
    }
}
