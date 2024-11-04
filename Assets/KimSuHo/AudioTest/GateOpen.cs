using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool audioPlay = false;
    void Start()
    {
        // 현재 게임 오브젝트에서 Animator 컴포넌트를 가져옴
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Animator가 존재하고 활성화된 경우
        if (animator != null && animator.enabled && !audioPlay)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.GateOpen);
            audioPlay = true;
            StartCoroutine(Opens());
        }
    }

    IEnumerator Opens ()
    {
        yield return new WaitForSeconds(0.5f);
        this.enabled = false;
    }
}
