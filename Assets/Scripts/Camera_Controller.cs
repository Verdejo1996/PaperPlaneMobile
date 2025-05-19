using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public float yOffset = 2f;
    public float minY = 1f; // Límite mínimo de altura

    private float initialZ;
    private float initialX;

    void Start()
    {
        initialZ = transform.position.z;
        initialX = transform.position.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetY = Mathf.Max(target.position.y + yOffset, minY);
        Vector3 desiredPosition = new(initialX, targetY, initialZ);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

}
