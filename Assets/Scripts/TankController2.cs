using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController2 : MonoBehaviour
{
    public Rigidbody2D Rb;
    public float speed=6;
    private float velx;
    private float vely;
    private Animator anim;
    public int health;
    private GameObject projectile;
    public GameObject vfx_destroy;

    public GameObject location;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        
        //if (networkID.IsMine)
        //{
            Camera camera = Camera.main;
           
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (networkID.IsMine)
        //{
            Physics2D.IgnoreLayerCollision(8, 9);
            Physics2D.IgnoreLayerCollision(8, 8);
            velx = Input.GetAxis("Horizontal");
            vely = Rb.velocity.y;
            Rb.velocity = new Vector2(velx * speed, vely);


            //Debug.Log(Rb.position);
            if (velx == 0)
            {
                anim.SetBool("isRunning", false);

            }
            else
                anim.SetBool("isRunning", true);

            if (health <= 0)
            {

                Destroy(gameObject);
            }
        //}
    }

    public void TakeDamage(int damage)
    {


        projectile = Instantiate(vfx_destroy, location.transform.position, location.transform.rotation) as GameObject;


        //Debug.Log(projectile.transform.position);
        health -= damage;
        HealthBarTank.healthTank -= damage;
        projectile = null;
    
            
    }
}
