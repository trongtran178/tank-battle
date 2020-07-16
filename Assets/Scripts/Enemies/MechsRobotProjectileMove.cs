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
        player = GameObject.FindGameObjectWithTag("player");
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
        {
            Debug.Log(collision.tag);
            switch (collision.tag)
            {
                case "player":
                    {
                        player.GetComponent<TankController2>()?.TakeDamage(30);
                        // player.GetComponent<TankController3D>()?.TakeDamage(30);

                        DestroyProjectile();
                        break;
                    }
                case "allies":
                case "allies_collider":
                    {
                        if (attackTarget != null)
                        {
                            if (attackTarget.GetComponentInChildren<Dogcollider>() != null)
                            {
                                attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(20);
                            }
                            else if (attackTarget.GetComponentInChildren<PlaneCollider>() != null)
                            {
                                attackTarget.GetComponentInChildren<PlaneCollider>().TakeDamage(20);
                            }
                            else if (attackTarget.GetComponentInChildren<EnemyTu>() != null)
                            {
                                attackTarget.GetComponentInChildren<EnemyTu>().TakeDamage(20);
                            }
                            Debug.Log("Destroy projectile");
                            DestroyProjectile();
                        }
                        break;
                    }
            }
        }

        if (collision.name == "GroundGrass")
        {
            DestroyProjectile();
        }

    }

    void DestroyProjectile()
    {
        // Instantiate(explosion, transform.position, transform.rotation);
        Destroy(self);
    }
}
