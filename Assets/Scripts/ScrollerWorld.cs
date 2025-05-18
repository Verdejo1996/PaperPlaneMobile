using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerWorld : MonoBehaviour
{
    public float scrollSpeed = 5f;
    //private bool isActive = false;

    void Update()
    {

        transform.Translate(scrollSpeed * Time.deltaTime * Vector3.back);

        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }

    }
}
