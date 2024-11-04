using JetBrains.Annotations;
using System;
using UnityEngine;

// 한번에 몰아서 하나씩 넣기
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ClearStampTile : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] BoxCollider collider;              // 상호작용을 할 수 있는 장소
    [SerializeField] Rigidbody rigid;                   // 트리거 작동에 필요한 rigidbody

    [Header("State")]
    [SerializeField] State curState;                    // 하나 삭제, 전체 삭제
    private enum State { ONE, ALL }

    [Header("PlayerStamps")]
    [SerializeField] StampType[] stamps = new StampType[6];   // 플레이어의 부착한 모든 면

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();

        Init();
    }

    private void Init()
    {
        // 초기 위치 box의 머리에
        gameObject.transform.localPosition = new Vector3(0, 0.55f, 0);
        gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        // 콜라이더는 트리거로
        collider.isTrigger = true;
        // 중력은 안받기로
        rigid.isKinematic = true;
        rigid.useGravity = false;
    }

    private void Start()
    {
        StampSetting();
    }

    /// <summary>
    /// 플레이어의 스탬프 면을 전부 지우기 위해 해당 스탬프들을 
    /// 게임 시작 시 자동으로 가져오는 함수
    /// </summary>
    private void StampSetting()
    {
        // 플레이어를 찾고
        GameObject player = GameObject.FindWithTag("Player");

        for (int index = 0; index < player.transform.childCount; index++)
        {
            // 플레이어의 자식 중에 StampPoint를 찾는데 있으면
            if (player.transform.GetChild(index).name.Equals("StampPoint"))
            {
                // StampPoint를 가져오고
                Transform stampPoints = player.transform.GetChild(index);
                // StampPoint 각 면에 있는 Stamp를
                for (int stampIndex = 0; stampIndex < stampPoints.childCount; stampIndex++)
                {
                    // stamps 배열에 다 넣는다 
                    stamps[stampIndex] = stampPoints.GetChild(stampIndex).GetComponent<StampType>();
                }
                // 다 넣었으면 반복문을 종료한다
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        StampType type = other.GetComponent<StampType>();

        if (type is not null)
        {
            if (curState.Equals(State.ONE))
            {
                ClearStempOne(type);
            }
            else
            {
                ClearStempAll();
            }
        }
    }

    // 스탬프 단면 지우기
    private void ClearStempOne(StampType type)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.WaterTile);
        type.ClearTile();
    }

    // 스탬프 다 지우기
    private void ClearStempAll()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.ResetTile);
        foreach (StampType type in stamps)
        {
            type.ClearTile();
        }
    }

}
