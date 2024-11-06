using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGround : MonoBehaviour
{
    // 플레이어 큐브의 레이어를 입력
    [SerializeField] LayerMask layer;
    // 큐브가 특정 상황에만 보이게 하기 위한 컴포넌트
    [SerializeField] MeshRenderer meshRenderer;
    // 빨간 스탬프
    [SerializeField] RedStamp redStamp;
    private void Update()
    {
        // 레이캐스트로 플레이어가 감지 될 때만 큐브 이미지를 활성화
        if(!Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 2f, layer) || !hit.transform.CompareTag("Player"))
        {
            meshRenderer.enabled = false;
            return;
        }
        meshRenderer.enabled = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌 시 해당 위치에 바닥을 생성
        if (!collision.transform.CompareTag("Player")) return;
        redStamp.UseRedGround(transform);
    }
}
