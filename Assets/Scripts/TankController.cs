using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    public Rigidbody2D backTire, frontTire,carRigidbody;
    private float movement;
    public float speed =100;
    public float carTorque=10;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        

        if (speed < 300)
        {
            speed *= -movement * Time.fixedDeltaTime;
        }
        else speed = 300;
        backTire.AddTorque(speed);
        frontTire.AddTorque(speed);
        if (carTorque < 100)
            carTorque *= -movement * Time.fixedDeltaTime;
        else carTorque = 10;
        carRigidbody.AddTorque(carTorque);
    }
}
