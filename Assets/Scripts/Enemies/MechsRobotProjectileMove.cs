using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MechsRobotProjectileMove : MonoBehaviour
{
    public GameObject attackTarget;
    public GameObject self;
    public GameObject explosion;
    public GameObject player;
    public GameObject firePoint;
    public Vector3 moveDir;
    public bool isFlip = false;
    public float speed;

    private float initializationTime;

    void Start()
    {
        // edit tag, hard code
        //player = GameObject.FindGameObjectWithTag("player");
        if (attackTarget)
        {
            moveDir = (attackTarget.transform.position - self.transform.position).normalized;
        }
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!self) return;

        self.transform.position += moveDir * speed * Time.deltaTime;
        self.transform.localRotation = firePoint.transform.localRotation;

        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (timeSinceInitialization >= 4)
        {
            DestroyProjectile();
            return;
        }
    }

    //private void FixedUpdate()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (collision == null) return;
            switch (collision.tag)
            {
                case "player":
                    {
                        attackTarget?.GetComponentInParent<TankController2>()?.TakeDamage(30);
                        attackTarget?.GetComponentInParent<TankController3D>()?.TakeDamage(20);

                        DestroyProjectile();
                        break;
                    }
                case "allies":
                case "allies_collider":
                    {
                        if (attackTarget != null)
                        {
                            if (attackTarget?.GetComponentInChildren<Dogcollider>() != null)
                            {
                                attackTarget?.GetComponentInChildren<Dogcollider>()?.TakeDamage(20);
                            }
                            else if (attackTarget?.GetComponentInChildren<PlaneCollider>() != null)
                            {
                                attackTarget?.GetComponentInChildren<PlaneCollider>()?.TakeDamage(20);
                            }
                            else if (attackTarget?.GetComponent<PlaneCollider>() != null)
                            {
                                attackTarget?.GetComponent<PlaneCollider>()?.TakeDamage(20);
                            }
                            else if (attackTarget.GetComponentInChildren<EnemyTu>() != null)
                            {
                                attackTarget?.GetComponentInChildren<EnemyTu>()?.TakeDamage(20);
                            }
                            Debug.Log("Destroy projectile");
                            DestroyProjectile();
                        }
                        break;
                    }
            }

            // Detect player 3D
            if (collision.name.Equals("player_collider"))
            {
                //collision.GetComponentInParent<TankController3D>()?.TakeDamage(20);
                collision?.GetComponentInParent<TankController3D>()?.TakeDamage(1);
                //player.GetComponent<TankController3D>()?.TakeDamage(1);
                DestroyProjectile();
            }

            if (collision.name == "GroundGrass")
            {
                DestroyProjectile();
            }
        }
        catch
        {
            Debug.Log("Catch error in robot projectile !");
        }
    }

    void DestroyProjectile()
    {
        // Instantiate(explosion, transform.position, transform.rotation);
        if (self)
            Destroy(self);
    }
}
