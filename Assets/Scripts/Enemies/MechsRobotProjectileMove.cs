﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MechsRobotProjectileMove : MonoBehaviour
{
    public GameObject attackTarget;
    public GameObject self;
    public GameObject explosion;
    public GameObject player;
    public GameObject firePoint;
    public bool isFlip = false;
    public float speed;

    private Vector2 attackTargetVector;
    private float initializationTime;

    // Start is called before the first frame update
    void Start()
    {   
        // edit tag, hard code
        player = GameObject.FindGameObjectWithTag("player");
        if (attackTarget)
        {
            attackTargetVector = new Vector3(1000.0f * (isFlip ? 1 : -1), firePoint.transform.position.y);
        }
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {

        if (!self) return;

        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (timeSinceInitialization >= 4)
        {
            DestroyProjectile();
            return;
        }
        self.transform.position = Vector3.MoveTowards(self.transform.position, attackTargetVector, speed * Time.deltaTime);
        self.transform.localRotation = firePoint.transform.localRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            Debug.Log(collision.tag);
            switch (collision.tag)
            {           
                case "player":
                    {
                        player.GetComponent<TankController2>().TakeDamage(20);
                        DestroyProjectile();
                        break;
                    }
                case "allies":
                    {
                        if(attackTarget != null)
                        {
                            if (attackTarget.GetComponentInChildren<Dogcollider>() != null) { 
                                attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(20);
                            }
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
