using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스탬프를 입히는 스크립트
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class StampType : MonoBehaviour
{
    [SerializeField] Type curType;         // 현재 이 면의 스탬프
    [SerializeField] SpriteRenderer render;     // 그림을 설정할 스프라이트
    [SerializeField] Rigidbody body;            // 스탬프의 충격을 설정할 RigidBody
    
    /// <summary>
    /// None, Red, Blue, Yellow
    /// 없음, 빨강, 파랑, 노랑
    /// </summary>
    public enum Type { None, Red, Blue, Yellow, Size }

    /// <summary>
    /// 현재 면의 스탬프의 종류를 가져오는 프로퍼티
    /// </summary>
    public Type GetStampType { get { return curType; } }

    /// <summary>
    /// 현재 면의 스탬프를 변경하는 프로퍼티
    /// </summary>
    public Type ChangeType { set { curType = value; ChangeSprite(); } }

    /// <summary>
    /// 스탬프의 이미지가 담겨있는 배열
    /// </summary>
    public Sprite[] stampSprites = new Sprite[(int)Type.Size];

    /// <summary>
    /// 스탬프를 초기화 시키는 함수
    /// </summary>
    public void ClearTile() => ChangeType = Type.None;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody>();

        body.useGravity = false;
        body.isKinematic = true;
    }

    private void ChangeSprite()
    {
        render.sprite = stampSprites[(int)curType];
        render.drawMode = SpriteDrawMode.Sliced;
        render.size = new Vector2(1, 1);
    }
}
