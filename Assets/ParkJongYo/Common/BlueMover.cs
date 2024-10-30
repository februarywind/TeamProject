using System.Collections;
using UnityEngine;

public class BlueMover : MonoBehaviour
{
    private float rotationAngle = 90f; // 회전할 각도
    private float rotationSpeed = 360f; // 초당 회전 속도
    private bool isRotating = false; // 회전 중인지 여부
    [SerializeField] CubeMove cubeMove;
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            // CubeMove 입력 막기
            cubeMove.enabled = false;
        }
        else if (!isRotating)
        {
            cubeMove.enabled = true;
        }

        if (isRotating || !Input.GetKey(KeyCode.E))
        {
            return; // 회전 중이거나 E 키가 눌리지 않으면 입력 무시
        }

        if (Input.GetKeyDown(KeyCode.A)) // A 키를 눌렀을 때
        {
            StartCoroutine(Rotate(cubeMove.transform, -rotationAngle)); // 왼쪽으로 90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.D)) // D 키를 눌렀을 때
        {
            StartCoroutine(Rotate(cubeMove.transform, rotationAngle)); // 왼쪽으로 90도 회전
        }
    }
    // 큐브 회전과 동시에 e키를 누를 시 cubeMove가 비활성화 되는 버그가 발생
    // 파란 스탬프 비 활성화 시 cubeMove를 true로 바꿔서 해결 함
    private void OnDisable()
    {
        if (!cubeMove) return;
        cubeMove.enabled = true;
    }

    private IEnumerator Rotate(Transform player, float angle)
    {
        isRotating = true; // 회전 중임을 표시
        float targetAngle = player.transform.eulerAngles.y + angle; // 목표 각도
        float currentAngle = player.transform.eulerAngles.y; // 현재 각도
        float rotSpeed = rotationSpeed * Time.deltaTime; // 회전 속도

        while (Mathf.Abs(targetAngle - currentAngle) > rotSpeed)
        {
            // 플레이어 오브젝트의 각도를 회전
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotSpeed);
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, currentAngle, player.transform.eulerAngles.z);
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 각도에 도달한 후 최종 각도로 설정
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, targetAngle, player.transform.eulerAngles.z);
        isRotating = false; // 회전 완료 표시
    }
}
