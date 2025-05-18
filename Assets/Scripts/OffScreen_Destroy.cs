using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreen_Destroy : MonoBehaviour
{
    void Update()
    {
        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }
}
