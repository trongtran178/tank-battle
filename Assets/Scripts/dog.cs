using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dog : MonoBehaviour
{

    private Animator ami;
    private Rigidbody2D rb;
    private float velx=2;
    private float vely = 0;

    // Start is called before the first frame update
    void Start()
    {
        ami = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        vely = rb.velocity.y;
        rb.velocity = new Vector2(velx, vely);

        if (velx > 0)
        {
            ami.SetBool("isWalk", true);
        }

    }

  
}
