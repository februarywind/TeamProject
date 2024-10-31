using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///  노란색 스탬프 확인 클래스 
/// </summary>
public class YellowChecker : MonoBehaviour
{
    [SerializeField] bool canMove;          // 돌진 가능 여부
    [SerializeField] LayerMask layerMask;   // 확인할 레이어 마스크

    [SerializeField] float distance;        // 레이캐스트 거리
    [SerializeField] GameObject destroyTarget;  // 능력 사용 중 파괴할 타겟

    /// <summary>
    /// 돌진 가능 여부를 가져오는 프로퍼티
    /// </summary>
    public bool CanMove { get { return canMove; } private set { } }
    public void DestroyTarget() { if(destroyTarget is not null) Destroy(destroyTarget); }

    public void CheckRay()
    {
        RaycastHit hit;

        // Ray를 checker의 아래 방향으로 발사하고
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, distance, layerMask);

        // 해당 Ray에 걸린 오브젝트(Map)가 있으면 갈 수 있다
        canMove = (hit.collider) ? true : false;
       

        // 그런데 Map인데 ClearStampTile(리셋, 지우기) 타일이면?
        // 바로 앞에 멈추게 (못가게 해야한다)
        if (canMove
            && hit.collider.gameObject.GetComponent<ClearStampTile>() is not null
            && hit.collider.gameObject.GetComponent<ClearStampTile>().enabled.Equals(true))
        {
            canMove = false;
        }
        else if (canMove
            && hit.collider.gameObject.name.Contains("slope")) canMove = false;
        else if (canMove
            && hit.collider.gameObject.GetComponent<Elevator>() is not null) canMove = false;
     
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌을 했는데 해당 오브젝트가 파괴가 가능한 오브젝트이면
        if(other.gameObject.CompareTag("CrackedRock"))
        {
            // 파괴할 오브젝트를 담아놓기
            destroyTarget = other.gameObject;
            // 움직여서 파괴할 수 있으니 움직이 가능으로 설정
            canMove = true;
        }
        else canMove = false;
    }
}
