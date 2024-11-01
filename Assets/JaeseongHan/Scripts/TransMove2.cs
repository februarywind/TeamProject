using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransMove2 : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] Rigidbody[] bodies;
    [SerializeField] Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody body in bodies)
        {
            body.position = body.transform.position + Vector3.forward * Time.deltaTime;
            transform.position = transform.position + Vector3.forward * Time.deltaTime;
        }
    }

    private void Update()
    {
        transform.position = transform.position + Vector3.forward * Time.deltaTime;
    }
}
