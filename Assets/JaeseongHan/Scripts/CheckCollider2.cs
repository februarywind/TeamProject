using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

/// <summary>
/// 콜라이더 
/// </summary>
public class CheckCollider2 : MonoBehaviour
{
    [SerializeField] StampType2 type;               // 큐브에 달려있는 스탬프 확인
    [SerializeField] UnityEvent[] interaction = new UnityEvent[(int)StampType2.StampType.Size];      // 각 스탬프의 상호작용들
    [SerializeField] BoxCollider boxCollider;       // 콜라이더

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // 임시로 번호키를 누르면 변경
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            type.ChangeType = StampType2.StampType.None;   
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            type.ChangeType = StampType2.StampType.Red;   

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            type.ChangeType = StampType2.StampType.Blue;   
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            type.ChangeType = StampType2.StampType.Yellow;   
        }
        // E를 누를때 && 박스 콜라이더가 있을 때(rolling이 아니다)
        else if(Input.GetKeyDown(KeyCode.E) && boxCollider.enabled)
        {
            interaction[(int)type.GetStempType]?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StampType2 type = other.GetComponent<StampType2>();

        // type 컴포넌트가 있다 -> cube에 붙어 있는 Stemp
        if (type is not null)
        {
            this.type = type;
            Debug.Log($"enter object : {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StampType2 type = other.GetComponent<StampType2>();

        if (this.type.Equals(type))
        {
            this.type = null;
        }
    }
}
