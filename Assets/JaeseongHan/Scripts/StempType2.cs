using UnityEngine;

/// <summary>
/// 스탬프를 입히는 스크립트
/// </summary>
public class StempType2 : MonoBehaviour
{
    [SerializeField] StempType curType;         // 현재 이 면의 스탬프
    [SerializeField] SpriteRenderer render;     // 그림을 설정할 스프라이트

    /// <summary>
    /// None, Red, Blue, Yellow
    /// 없음, 빨강, 파랑, 노랑
    /// </summary>
    public enum StempType { None, Red, Blue, Yellow, Size }

    /// <summary>
    /// 현재 면의 스탬프의 종류를 가져오는 프로퍼티
    /// </summary>
    public StempType GetStempType { get { return curType; } }

    /// <summary>
    /// 현재 면의 스탬프를 변경하는 프로퍼티
    /// </summary>
    public StempType ChangeType { set { curType = value; ChangeSprite(); } }

    /// <summary>
    /// 스탬프의 이미지가 담겨있는 배열
    /// </summary>
    public Sprite[] stempSprites = new Sprite[(int)StempType.Size];

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void ChangeSprite()
    {
        render.sprite = stempSprites[(int)curType];
        render.drawMode = SpriteDrawMode.Sliced;
        render.size = new Vector2(1, 1);
    }
}
