using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryCube2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    public void Remove()
    {
        StartCoroutine(RemoveRoutine());
    }

    private IEnumerator RemoveRoutine()
    {
        float time = 0f;
        yield return null;
        while(time < 0.05f)
        {
            transform.Translate(Vector3.down * moveSpeed);
            time += Time.deltaTime;
        }

        Destroy(gameObject, 1f);
    }
}
