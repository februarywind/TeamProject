using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sdsadsad : MonoBehaviour
{
    private float rotationAngle = 90f; // 회전할 각도
    private float rotationSpeed = 360f; // 초당 회전 속도

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // A 키를 눌렀을 때
        {
            StartCoroutine(Rotate(-rotationAngle)); // 왼쪽으로 90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // D 키를 눌렀을 때
        {
            StartCoroutine(Rotate(rotationAngle)); // 오른쪽으로 90도 회전
        }
    }

    private IEnumerator Rotate(float angle)
    {
        float targetAngle = transform.eulerAngles.y + angle; // 목표 각도
        float currentAngle = transform.eulerAngles.y; // 현재 각도
        float rotSpeed = rotationSpeed * Time.deltaTime; // 회전 속도

        //Mathf.Abs(절대값 반환) 음수일때 양수, 양수 일때 양수
        while (Mathf.Abs(targetAngle - currentAngle) > rotSpeed)
        {
            // Mathf.MoveTowardsAngle(현재 각도, 목표 각도, 회전속도)
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 각도에 도달한 후 최종 각도로 설정
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }
}
