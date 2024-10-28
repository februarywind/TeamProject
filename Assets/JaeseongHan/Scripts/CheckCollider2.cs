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
    [SerializeField] StempType2 type;               // 큐브에 달려있는 스탬프 확인
    [SerializeField] UnityEvent[] interaction = new UnityEvent[(int)StempType2.StempType.Size];      // 각 스탬프의 상호작용들
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
            type.ChangeType = StempType2.StempType.None;   
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            type.ChangeType = StempType2.StempType.Red;   

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            type.ChangeType = StempType2.StempType.Blue;   
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            type.ChangeType = StempType2.StempType.Yellow;   
        }
        // E를 누를때 && 박스 콜라이더가 있을 때(rolling이 아니다)
        else if(Input.GetKeyDown(KeyCode.E) && boxCollider.enabled)
        {
            interaction[(int)type.GetStempType]?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StempType2 type = other.GetComponent<StempType2>();

        // type 컴포넌트가 있다 -> cube에 붙어 있는 Stemp
        if (type is not null)
        {
            this.type = type;
            Debug.Log($"enter object : {other.gameObject.name}");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StempType2 type = other.GetComponent<StempType2>();

        if (this.type.Equals(type))
        {
            this.type = null;
        }
    }
}
