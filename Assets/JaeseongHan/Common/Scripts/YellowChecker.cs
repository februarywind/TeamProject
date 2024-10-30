using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  노란색 스탬프 확인 클래스 
/// </summary>
public class YellowChecker : MonoBehaviour
{
    [SerializeField] bool canMove;          // 돌진 가능 여부
    [SerializeField] LayerMask layerMask;   // 확인할 레이어 마스크

    [SerializeField] float distance;        // 레이캐스트 거리

    /// <summary>
    /// 돌진 가능 여부를 가져오는 프로퍼티
    /// </summary>
    public bool CanMove { get { return canMove; } private set { } }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        // Todo: 레이어 충돌을 Map만 -> 코드 간략화
        canMove = false;
    }
}
