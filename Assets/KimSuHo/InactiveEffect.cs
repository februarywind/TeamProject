using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveEffect : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool audioPlay = false;
    void Start()
    {
        // 부모 오브젝트에서 Animator 컴포넌트를 가져옴
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        // Animator가 존재하고 활성화된 경우
        if (animator != null && animator.enabled && !audioPlay)
        {
            EffectManager.Instance.PlayEffect(EffectManager.Effect.InactiveTile, transform.position);
            audioPlay = true;
            StartCoroutine(Opens());
        }
    }

    IEnumerator Opens()
    {
        yield return new WaitForSeconds(1.2f);
        this.enabled = false;
    }
}
