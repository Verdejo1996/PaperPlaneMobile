using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat_Controller : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Si el jugador está cerca, sigue al jugador
            transform.position = player.position;
        }
    }
}
