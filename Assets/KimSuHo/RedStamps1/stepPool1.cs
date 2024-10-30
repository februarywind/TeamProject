using System.Collections.Generic;
using UnityEngine;

public class stepPool1 : MonoBehaviour
{
    public GameObject stepPrefab; // 발판 프리팹
    public int poolSize = 7; // 풀 크기
    private Queue<GameObject> stepPool = new Queue<GameObject>();

    void Start()
    {
        // 초기 발판 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject step = Instantiate(stepPrefab);
            step.SetActive(false); // 비활성화 상태로 설정
            stepPool.Enqueue(step);
        }
    }

    public GameObject GetStep()
    {
        if (stepPool.Count > 0)
        {
            GameObject step = stepPool.Dequeue();
            step.SetActive(true); // 활성화
            return step;
        }
        return null; // 풀에 더 이상 발판이 없을 경우
    }

    public void ReturnStep(GameObject step)
    {
        step.SetActive(false); // 비활성화
        stepPool.Enqueue(step);
    }
}
