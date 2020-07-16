using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    private float velx=2f;
    private float vely=0f;
    public float speed;
    private GameObject projectile;
    public GameObject vfx_destroy;
    public GameObject location;
    public GameObject tank;
    private Vector3 kc;
    public float distanceMax;
    public float distanceMin;

    public Image healthyBar;
    public float maxHealthy = 100f;
    public float health;
    GunEnemy gunEnemy;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealthy;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        healthyBar.fillAmount = health / maxHealthy;
        if (tank != null)
        {
            kc = tank.transform.position - transform.position;
        }
            float doLonKc = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));
    
        vely = rb.velocity.y;
        rb.velocity = new Vector2(velx, vely);
        if (velx == 0)
        {
            anim.SetBool("isRunning", false);
            
        }
        else
        {
            anim.SetBool("isRunning", true);
           
        }

        //&& trajectoryScript.flagShoot == false
        //if (GunEnemy.timeGun > 0)
        //{
        if (doLonKc > distanceMax)
        {
            velx = -speed;
            Debug.Log("lui");
        }
        else if (doLonKc < distanceMin)
        {
            velx = speed;
            Debug.Log("tien");

        }
        else velx = 0;
        //}
        //else
        //{
        //    velx = 0;
        //}

        if (health <= 0)
        {
            
            Destroy(gameObject);
        }
    }

    //public void Dizzy()
    //{
    //    isDizzy = true;
    //    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    //    ami.SetBool("isWalk", false);
    //    ami.SetBool("isAttack", false);
    //    Invoke("DizzyFinished", 4.0f);
    //}

    //private void DizzyFinished()
    //{
    //    isDizzy = false;
    //    rb.constraints = RigidbodyConstraints2D.None;
    //    ami.SetBool("isWalk", true);
    //    ami.SetBool("isAttack", false);
    //}


    public void TakeDamage(float damage)
    {

 
        projectile = Instantiate(vfx_destroy, location.transform.position, location.transform.rotation) as GameObject;
        //projectile.transform.position = transform.position;

        
        health -= damage;
   
        projectile = null;
    }
}
