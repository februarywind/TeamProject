using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable2 : MonoBehaviour
{
    public void OpenDoor()
    {
        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        float time = 5f;
        float cutTime = 0f;
        while (cutTime < time)
        {
            transform.position += Vector3.down * Time.deltaTime;
            cutTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
