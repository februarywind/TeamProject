using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCube : MonoBehaviour
{
    // 내려가는 속도
    [SerializeField] float moveSpeed = 10f;

    public void Remove()
    {
        // 삭제하라고 명령이 오면 삭제 시작
        StartCoroutine(RemoveRoutine());
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Destruction);
    }

    private IEnumerator RemoveRoutine()
    {
        float time = 0f;
        yield return null;
        while (time < 0.05f)    // 0.05초 동안 내려가기
        {
            transform.Translate(Vector3.down * moveSpeed);
            time += Time.deltaTime;
        }
        
        // Todo: Disable vs Destroy 뭐가 GCC에 좋은지 판단 후 수정
        Destroy(gameObject, 1f);    // 오브젝트는 넉넉하게 1초 후 삭제
    }
}
