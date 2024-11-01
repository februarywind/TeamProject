using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 스탬프 위치 열거형
enum StampDir
{
    Up, Forward, Back, Right, Left
}

// 한번에 몰아서 하나씩 넣기
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class StampTile : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] StampType.Type stampType;    // 이 타일의 상호작용할 때 사용될 스탬프
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
    public Sprite[] stampSprites = new Sprite[(int)StampType.Type.Size];

    // 스탬프 방향 설정
    [SerializeField] StampDir _stampDir;
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
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
        // 게임 시작 시 설정한 타입에 맞게 스프라이트 설정
        render.sprite = stampSprites[(int)stampType];
        render.drawMode = SpriteDrawMode.Sliced;
        render.size = new Vector2(0.8f, 0.8f);
        switch (_stampDir)
        {
            case StampDir.Up:
                break;
            case StampDir.Forward:
                transform.parent.rotation = Quaternion.Euler(90f, 0f, 0f);
                break;
            case StampDir.Back:
                transform.parent.rotation = Quaternion.Euler(-90f, 0f, 0f);
                break;
            case StampDir.Right:
                transform.parent.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            case StampDir.Left:
                transform.parent.rotation = Quaternion.Euler(-0f, 0f, 90f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        StampType type = other.gameObject.GetComponent<StampType>();

        // type != null -> 스탬프가 충돌했다
        if (type is not null)
        {
            // 획득이면 해당 면에 스탬프 부착
            if (tileType.Equals(StampTileType.Get)) type.ChangeType = stampType;
            // 획득이 아니고(=> 사용이고) 타일이 같다면 
            else if (stampType.Equals(type.GetStampType))
            {
                // 한번 상호작용하면 더이상 안되게
                collider.enabled = false;
                // 상호작용 함수 실행
                tileInteraction?.Invoke();
            }
        }
    }
}
