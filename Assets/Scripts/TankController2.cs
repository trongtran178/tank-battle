using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    private bool isDizzy = false;
    public GameObject location;
    private trajectoryScript trajectoryHandleScript;
    // 
    public GameObject bulletsAndAllies;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        trajectoryHandleScript = GetComponentInChildren<trajectoryScript>();
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

            if (isDizzy) return;
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

        health -= damage;
        HealthBarTank.healthTank -= damage;
        projectile = null; 
    }

    public void Dizzy()
    {
        isDizzy = true;
        Rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        anim.SetBool("isRunning", false);
        trajectoryHandleScript.enabled = false;
        ManaArmy[] manaArmies = bulletsAndAllies.GetComponentsInChildren<ManaArmy>();
        for (int i = 0; i < manaArmies.Length; i++)
        {
            manaArmies[i].isLock = true;
        }

        Invoke("DizzyFinished", 4.0f);
    }

    private void DizzyFinished()
    {
        isDizzy = false;
        Rb.constraints = RigidbodyConstraints2D.None;
        trajectoryHandleScript.enabled = true;
        ManaArmy[] manaArmies = bulletsAndAllies.GetComponentsInChildren<ManaArmy>();
        for (int i = 0; i < manaArmies.Length; i++)
        {
            manaArmies[i].isLock = false;
        }
        anim.SetBool("isRunning", true);
    }

}
