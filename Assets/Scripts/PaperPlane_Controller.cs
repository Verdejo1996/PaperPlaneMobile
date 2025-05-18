using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class PaperPlane_Controller : MonoBehaviour
{
    public Game_Controller game_Controller;
    public ScrollerWorld scrollerWorld;
    public Slider powerSlider;
    public float minPower = 5f;
    public float maxPower = 20f;
    public float chargeSpeed = 0.5f;
    public Rigidbody planeRb;

    public bool hasLaunched = false;
    private bool charging = false;
    private float power = 0f;
    public float liftForce = 5f;

    public float glideForce = 5f;          // Fuerza que empuja hacia adelante

    public float maxSpeed = 10f;           // Velocidad máxima de planeo

    private void Start()
    {
        
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
        else
        {
            GlidePlane();
        }
    }

    void GlidePlane()
    {
        Vector3 posInicial = transform.position;

        float time = Time.deltaTime * 0.1f;

        posInicial.y -= time;

        transform.position = posInicial;
            
    }

    void LaunchPlane()
    {
        hasLaunched = true;

        game_Controller.StartGame();
    }

    void ApplyGlidingPhysics()
    {
/*        // Empuja suavemente hacia adelante
        Vector3 forwardForce = transform.forward * glideForce;
        if (planeRb.velocity.magnitude < maxSpeed)
        {
            planeRb.AddForce(forwardForce);
        }

        // Agrega una fuerza de "sustentación" hacia arriba proporcional a la velocidad
        Vector3 lift = Vector3.up * liftForce * Mathf.Clamp01(planeRb.velocity.magnitude / maxSpeed);
        planeRb.AddForce(lift, ForceMode.Force);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AirStream"))
        {
            planeRb.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
        }
    }
}
