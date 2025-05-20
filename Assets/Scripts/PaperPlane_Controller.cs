using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;
using UnityEngine.UI;

public class PaperPlane_Controller : MonoBehaviour
{
    public Game_Controller game_Controller;
    public ScrollerWorld scrollerWorld;
    public Slider powerSlider;
    public Rigidbody planeRb;
    public TextMeshProUGUI distanceText;
    private Vector3 posInicial;

    public float minPower = 5f;
    public float maxPower = 20f;
    public float chargeSpeed = 0.5f;

    public bool hasLaunched = false;
    private float distanceTravelled;
    private bool charging = false;
    private float power = 0f;
    public float liftForce = 5f;

    public float glideForce = 5f;          // Fuerza que empuja hacia adelante
    public float maxSpeed = 10f;           // Velocidad máxima de planeo

    private float glideTime;


    private void Start()
    {
        distanceTravelled = 0;

    }

    void Update()
    {
        // Solo si aún no hemos lanzado
        if (!hasLaunched)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 pos = touch.position;

                // Solo si el dedo está en la parte derecha de la pantalla (zona de carga)
                if (pos.x > Screen.width * 0.7f)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        charging = true;
                        power = 0f;
                    }

                    if (touch.phase == TouchPhase.Moved && charging)
                    {
                        power += chargeSpeed * Time.deltaTime;
                        power = Mathf.Clamp(power, 0f, 1f);
                        powerSlider.value = power;
                    }

                    if (touch.phase == TouchPhase.Ended && charging)
                    {
                        charging = false;

                        LaunchPlane();
                        //ApplyGlidingPhysics();
                    }
                }
            }
        }
        else if(!game_Controller.beforeStart && Time.timeScale == 1)
        {
            GlidePlane();
            ReturnPosZ();
            distanceText.text = distanceTravelled.ToString();
        }
    }

    void GlidePlane()
    {
        posInicial = transform.position;
        glideTime = Time.deltaTime * 0.1f;

        posInicial.y -= glideTime;

        transform.position = posInicial;
        distanceTravelled++;
    }

    void ReturnPosZ()
    {
        posInicial = transform.position;
        float zReturn = Time.deltaTime * 0.5f;

        if(posInicial.z <= 1.2f)
        {
            zReturn = 0f;
        }
        
        posInicial.z -= zReturn;
        transform.position = posInicial;
        
    }

    void LaunchPlane()
    {
        hasLaunched = true;
        planeRb.velocity = 5 * Vector3.forward;

        StartCoroutine(StarGameAfterDelay());
    }

    IEnumerator StarGameAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        game_Controller.StartGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 posInicial = transform.position;

        if (other.CompareTag("AirStream"))
        {
            posInicial.y += 3f;
            transform.position = posInicial;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            Time.timeScale = 0f;
            game_Controller.endGame = true;
            game_Controller.GameOver();
        }
        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Decoration") || collision.collider.CompareTag("Tree"))
        {
            planeRb.useGravity = true;
            hasLaunched = false;
        }
    }
}
