using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMover : MonoBehaviour
{
    private float rotationAngle = 90f; // 회전할 각도
    private float rotationSpeed = 360f; // 초당 회전 속도
    private bool isRotating = false; // 회전 중인지 여부
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CubeMove cubeMove = player.GetComponent<CubeMove>();
            if (cubeMove != null)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    cubeMove.enabled = false; // CubeMove 컴포넌트 비활성화
                }
                else
                {
                    cubeMove.enabled = true; // CubeMove 컴포넌트 활성화
                }
            }

            if (isRotating || !Input.GetKey(KeyCode.E))
            {
                return; // 회전 중이거나 E 키가 눌리지 않으면 입력 무시
            }

            if (Input.GetKeyDown(KeyCode.A)) // A 키를 눌렀을 때
            {
                StartRotation(player, -rotationAngle); // 왼쪽으로 90도 회전
            }
            else if (Input.GetKeyDown(KeyCode.D)) // D 키를 눌렀을 때
            {
                StartRotation(player, rotationAngle); // 오른쪽으로 90도 회전
            }
        }
    }

    private void StartRotation(GameObject player, float angle)
    {
            StartCoroutine(Rotate(player, angle)); // 회전 시작
    }

    private IEnumerator Rotate(GameObject player, float angle)
    {
        isRotating = true; // 회전 중임을 표시
        float targetAngle = player.transform.eulerAngles.y + angle; // 목표 각도
        float currentAngle = player.transform.eulerAngles.y; // 현재 각도
        float rotSpeed = rotationSpeed * Time.deltaTime; // 회전 속도

        while (Mathf.Abs(targetAngle - currentAngle) > rotSpeed)
        {
            // 플레이어 오브젝트의 각도를 회전
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotSpeed);
            player.transform.eulerAngles = new Vector3(0, currentAngle, 0); // X와 Z를 0으로 설정
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 각도에 도달한 후 최종 각도로 설정
        player.transform.eulerAngles = new Vector3(0, targetAngle, 0); // X와 Z를 0으로 설정
        isRotating = false; // 회전 완료 표시
    }
}
