using UnityEngine;
using UnityEngine.Events;

/// <summary>
///  노란색 스탬프 확인 클래스 
/// </summary>
public class YellowChecker : MonoBehaviour
{
    [SerializeField] bool canMove;          // 돌진 가능 여부
    [SerializeField] LayerMask layerMask;   // 확인할 레이어 마스크

    [SerializeField] float distance;        // 레이캐스트 거리
    [SerializeField] GameObject destroyTarget;  // 능력 사용 중 파괴할 타겟

    [SerializeField] UnityEvent destroyCubeEvent;   // 파괴할 타겟에게 파괴하라고 명령하는 이벤트

    /// <summary>
    /// 돌진 가능 여부를 가져오는 프로퍼티
    /// </summary>
    public bool CanMove { get { return canMove; } private set { } }
    public void DestroyTarget()
    {
        if (destroyTarget is not null) destroyCubeEvent?.Invoke();
    }

    public void CheckRay()
    {
        // 일단 초기화 
        destroyCubeEvent.RemoveAllListeners();
        RaycastHit hit;

        // Ray를 checker의 아래 방향으로 발사하고
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, distance, layerMask);

        // 해당 Ray에 걸린 오브젝트(Map)가 있으면 갈 수 있다
        canMove = (hit.collider) ? true : false;


        // 만약 경사로 타일이 있으면 해당 위로는 못가게
        if (canMove
            && hit.collider.gameObject.name.Contains("slope")) canMove = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌을 했는데 해당 오브젝트가 파괴가 가능한 오브젝트이면
        if (other.gameObject.CompareTag("CrackedRock"))
        {
            // 파괴할 오브젝트를 담아놓기
            destroyTarget = other.gameObject;
            // 움직여서 파괴할 수 있으니 움직이 가능으로 설정
            canMove = true;

            DestroyCube script = other.gameObject.GetComponent<DestroyCube>();
            // 혹시 모를 한번 더 검증
            if (script is not null)
            {
                destroyCubeEvent.AddListener(script.Remove);
            }
        }
        else canMove = false;
    }
}
