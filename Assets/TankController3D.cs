using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController3D : MonoBehaviour
{
    public Rigidbody2D Rb;
    public float speed = 6;
    private float velx;
    private float vely;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject tank= GameObject.Find("TankPzIV");

        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velx = Input.GetAxis("Horizontal");
        vely = Rb.velocity.y;
        Rb.velocity = new Vector2(speed, vely);

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoveForward", true);
            anim.SetBool("isMoveBack", false);
            speed = 6;


        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMoveForward", false);
            anim.SetBool("isMoveBack", true);
            speed = -6;
            
        }
        else {
            anim.SetBool("isMoveForward", false);
            anim.SetBool("isMoveBack", false);
            speed = 0;
        }
        
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    anim.SetBool("isHit", true);
        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    anim.SetBool("isHit", false);

        //}
    }
}
