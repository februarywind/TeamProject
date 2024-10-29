using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 콜라이더에 충돌하는 스탬프를 확인
/// </summary>
public class CheckStampCollider : MonoBehaviour
{
    [SerializeField] StampType type;               // 큐브에 달려있는 스탬프 확인
    [SerializeField] BoxCollider boxCollider;       // 콜라이더
    [SerializeField] UseStampAbility useStampAbility;  // 스탬프 능력을 관리하는 스크립트

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StampType type = other.GetComponent<StampType>();

        // type 컴포넌트가 있다 -> cube에 붙어 있는 Stamp
        if (type is not null)
        {
            this.type = type;
            useStampAbility.ChangeAbility(type.GetStampType);
            Debug.Log($"enter object : {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StampType type = other.GetComponent<StampType>();

        if (this.type.Equals(type))
        {
            this.type = null;
        }
    }
}
