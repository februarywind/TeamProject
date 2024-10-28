using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.AI;
using UnityEngine.Tilemaps;

// 한번에 몰아서 하나씩 넣기
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ClearStempTile2 : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] BoxCollider collider;              // 상호작용을 할 수 있는 장소
    [SerializeField] Rigidbody rigid;                   // 트리거 작동에 필요한 rigidbody

    [Header("State")]
    [SerializeField] State curState;                    // 하나 삭제, 전체 삭제
    private enum State { ONE , ALL }

    [Header("PlayerStemps")]
    [SerializeField] StempType2[] stemps = new StempType2[6];   // 플레이어의 부착한 모든 면

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


    private void OnTriggerEnter(Collider other)
    {
        StempType2 type = other.GetComponent<StempType2>();

        if(type is not null)
        {
            if(curState.Equals(State.ONE))
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
    private void ClearStempOne(StempType2 type)
    {
        type.ClearTile();
    }

    // 스탬프 다 지우기
    private void ClearStempAll()
    {
        foreach(StempType2 type in stemps)
        {
            type.ClearTile();
        }
    }
    
}
