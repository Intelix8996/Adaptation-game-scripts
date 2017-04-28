using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemPhysicsOff : MonoBehaviour {

    private Rigidbody rb;
    [SerializeField]
    [Range(0, 40f)]
    private float offTime = 15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("PhysicsOff", offTime);
    }

    void PhysicsOff()
    {
        if (rb)
            Destroy(rb);
    }
}
