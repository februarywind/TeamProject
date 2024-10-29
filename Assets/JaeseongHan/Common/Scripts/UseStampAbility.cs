using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 능력을 사용하게 해주는 상태 패턴을 관리해주는 클래스
/// </summary>
public class UseStampAbility : MonoBehaviour
{
    [Tooltip("None(0), Red(1), Blue(2), Yellow(3)")]
    [SerializeField] StampType.Type curType;
    private StampBaseState[] states = new StampBaseState[(int)StampType.Type.Size];

    //  Todo: 스크립트 병합시 사용
    //  [Header("State Reference Field")]
    //  [SerializeField] RedMover redMover;
    //  [SerializeField] BlueMover blueMover;
    //  [SerializeField] YellowMover yellowMover;

    /// <summary>
    /// 스탬프가 변경이 되었을 때 변경작업을 해주는 함수
    /// </summary>
    /// <param name="type">변경될 스탬프의 종류</param>
    public void ChangeAbility(StampType.Type type)
    {
        states[(int)curType].Exit();
        curType = type;
        states[(int)curType].Enter();
    }

    private void Awake()
    {
        // 처음 시작할 때 면에 아무것도 없음
        curType = StampType.Type.None;
    }

    private void Start()
    {
        // Todo: 스크립트 병합시 사용
        // redMover = new RedMover(this);
        // blueMover = new BlueMover(this);
        // yellowMover = new YellowMover(this);

        // 스크립트를 따로 생성하는거라 Awake 타이밍에 없을 수도 있어서
        // Start에서 추가
        states[(int)StampType.Type.None] = new NoneAblicity(this);
        states[(int)StampType.Type.Red] = new NoneAblicity(this);
        states[(int)StampType.Type.Blue] = new NoneAblicity(this);
        states[(int)StampType.Type.Yellow] = new NoneAblicity(this);

        states[(int)curType].Enter();
    }

    private void Update()
    {
        states[(int)curType].Update();
    }

    /// <summary>
    /// 스탬프가 없는 상태일 때 -> 능력이 없는 상태
    /// </summary>
    private class NoneAblicity : StampBaseState
    {
        // 작동할 로직은 따로 없음
        public NoneAblicity(UseStampAbility parent) : base(parent)
        {
        }
    }
}
