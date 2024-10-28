using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태 패턴의 베이스 클래스
/// </summary>
public class BaseState2
{
    /// <summary>
    /// MonoBehaviour를 가지고 있는 상태 패턴의 주체
    /// </summary>
    protected UseStampAbility2 parent;

    public BaseState2(UseStampAbility2 parent)
    {
        this.parent = parent;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
