using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스탬프 상태 패턴의 베이스 클래스
/// </summary>
public class StampBaseState
{

    /// <summary>
    /// MonoBehaviour를 가지고 있는 상태 패턴의 주체
    /// </summary>
    protected UseStampAbility parent;

    public StampBaseState(UseStampAbility parent)
    {
        this.parent = parent;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
