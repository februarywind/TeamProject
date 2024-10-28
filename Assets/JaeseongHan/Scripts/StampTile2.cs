using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 한번에 몰아서 하나씩 넣기
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class StampTile2 : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] StampType2.StampType stampType;    // 이 타일의 상호작용할 때 사용될 스탬프
    [SerializeField] StampTileType tileType;            // 스탬프 상호작용 타입
    [SerializeField] BoxCollider collider;              // 상호작용을 할 수 있는 장소
    [SerializeField] Rigidbody rigid;                   // 트리거 작동에 필요한 rigidbody


    [Header("Set")]
    [SerializeField] UnityEvent tileInteraction;        // 상호작용할 이벤트
    public enum StampTileType { Get, Set }              // 획득, 사용

    [SerializeField] SpriteRenderer render;     // 그림을 설정할 스프라이트

    /// <summary>
    /// 스탬프의 이미지가 담겨있는 배열
    /// </summary>
    public Sprite[] stampSprites = new Sprite[(int)StampType2.StampType.Size];

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();

        Init();
    }

    private void Init()
    {
        // 초기 위치 box의 머리에
        gameObject.transform.localPosition = new Vector3 (0, 0.55f, 0);
        gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        // 콜라이더는 트리거로
        collider.isTrigger = true;
        // 중력은 안받기로
        rigid.isKinematic = true;
        rigid.useGravity = false;
    }

    private void Start()
    {
        render.sprite = stampSprites[(int)stampType];
        render.drawMode = SpriteDrawMode.Sliced;
        render.size = new Vector2(0.8f, 0.8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        StampType2 type = other.gameObject.GetComponent<StampType2>();
        Debug.Log(other.gameObject.name);
        if(type is not null)
        {
            // 획득이면 해당 면에 스탬프 부착
            if (tileType.Equals(StampTileType.Get)) type.ChangeType = stampType;
            // 획득이 아니고(=> 사용이고) 타일이 같다면 
            else if (stampType.Equals(type.GetStempType))
            {
                // 한번 상호작용하면 더이상 안되게
                collider.enabled = false;
                // 상호작용 함수 실행
                tileInteraction?.Invoke();
            }
        }
    }
}
