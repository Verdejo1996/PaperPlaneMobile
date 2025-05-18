using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro_Controller : MonoBehaviour
{
    public float tiltSpeed = 3f;
    private Rigidbody rb;

    void Start()
    {
        Input.gyro.enabled = true;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float tilt = Input.gyro.rotationRateUnbiased.y;
        rb.AddForce(tilt * tiltSpeed * Vector3.right, ForceMode.Force);
    }
}
