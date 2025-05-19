using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro_Controller : MonoBehaviour
{
    public float tiltSpeed = 3f;
    private Rigidbody rb;
    private Game_Controller controller;

    void Start()
    {
        Input.gyro.enabled = true;
        rb = GetComponent<Rigidbody>();
        controller = FindAnyObjectByType<Game_Controller>();
    }

    void FixedUpdate()
    {
        if (!controller.beforeStart)
        {
            float tilt = Input.gyro.rotationRateUnbiased.y;
            rb.AddForce(tilt * tiltSpeed * Vector3.right, ForceMode.Force);
        }
    }
}
