using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMover3 : MonoBehaviour
{
    private float rotationAngle = 90f; // 회전할 각도
    private float rotationSpeed = 360f; // 초당 회전 속도
    private bool isRotating = false; // 회전 중인지 여부
    private float rotationTolerance = 0.1f; // 회전 허용 오차

    void Update()
    {
        CubeMove4 cubeMove = transform.parent.GetComponent<CubeMove4>();
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
            StartRotation(-rotationAngle); // 왼쪽으로 90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.D)) // D 키를 눌렀을 때
        {
            StartRotation(rotationAngle); // 오른쪽으로 90도 회전
        }
    }

    private void StartRotation(float angle)
    {
        if (IsRotationZero())
        {
            StartCoroutine(Rotate(angle)); // 회전 시작
        }
    }

    private bool IsRotationZero()
    {
        // 부모 오브젝트의 X와 Z 회전이 0인지 확인 (허용 오차 포함)
        bool isXZero = Mathf.Abs(transform.parent.eulerAngles.x) < rotationTolerance;
        bool isZZero = Mathf.Abs(transform.parent.eulerAngles.z) < rotationTolerance;

        return isXZero && isZZero;
    }

    private IEnumerator Rotate(float angle)
    {
        isRotating = true; // 회전 중임을 표시
        float targetAngle = transform.parent.eulerAngles.y + angle; // 목표 각도
        float currentAngle = transform.parent.eulerAngles.y; // 현재 각도
        float rotSpeed = rotationSpeed * Time.deltaTime; // 회전 속도

        while (Mathf.Abs(targetAngle - currentAngle) > rotSpeed)
        {
            // 부모 오브젝트의 각도를 회전
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotSpeed);
            transform.parent.eulerAngles = new Vector3(0, currentAngle, 0); // X와 Z를 0으로 설정
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 각도에 도달한 후 최종 각도로 설정
        transform.parent.eulerAngles = new Vector3(0, targetAngle, 0); // X와 Z를 0으로 설정
        isRotating = false; // 회전 완료 표시
    }
}
