using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTile : MonoBehaviour
{
    public Transform targetPosition; // 엘리베이터가 이동할 목표 위치
    public Transform pos;            // 현재 엘리베이터 위치
    public float speed = 2f; // 이동 속도
    private bool playerOnTile = false; // 플레이어가 타일 위에 있는지 여부

    void Update()
    {
        if (playerOnTile)
        {
            // 엘리베이터를 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌했을 때
        {
            playerOnTile = true; // 플레이어가 타일 위에 있다고 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 타일에서 나갔을 때
        {
            playerOnTile = false; // 플레이어가 타일 위에 없다고 설정
        }
    }
}
