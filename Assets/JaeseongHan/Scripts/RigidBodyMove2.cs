using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMove2 : MonoBehaviour
{
    [SerializeField] Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        body.position = transform.position + Vector3.forward * Time.fixedDeltaTime;
    }
}
