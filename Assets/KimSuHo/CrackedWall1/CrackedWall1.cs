using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall1 : MonoBehaviour
{
    public GameObject[] smallCubes; // 작은 큐브 배열
    public float bombPower = 500f; // 폭발 힘
    public float bombRadius = 5f; // 폭발 반경
    public float bombY = 0f;        // 폭발 수직 방향 힘

    public void Explode()
    {
        foreach (GameObject all in smallCubes)
        {
            // 작은 큐브를 활성화하고 힘을 가함
            Rigidbody rb = all.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 폭발 방향 설정 (큐브의 위치에서 랜덤 방향으로 날리기)
                Vector3 direction = (all.transform.position - transform.position).normalized;
                rb.AddExplosionForce(bombPower, transform.position, bombRadius, bombY);
            }

            // 추가적으로, 일정 시간 후 파괴하는 코루틴 실행
            StartCoroutine(DestroyAfterTime(all, 0.5f));
        }
    }

    private IEnumerator DestroyAfterTime(GameObject all, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(all); // 일정 시간 후 파괴
    }
}
