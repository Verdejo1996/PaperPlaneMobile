using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerWorld : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float addSpeed = 3f;
    public float difficultyInterval = 5f;
    private float nextDifficultyTime;


    private void Start()
    {
        nextDifficultyTime = Time.time + difficultyInterval;
    }
    void Update()
    {

        transform.Translate(scrollSpeed * Time.deltaTime * Vector3.back);

        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }

        if(Time.time >= nextDifficultyTime)
        {
            scrollSpeed += addSpeed;
            nextDifficultyTime = Time.time + difficultyInterval;
        }

    }
}
