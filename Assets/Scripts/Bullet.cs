﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    
    public int damage = 20;
    //public static bool flagPlane = false;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        PlaneCollider plane = collision.GetComponent<PlaneCollider>();
        EnemyTu alliesTank= collision.GetComponent<EnemyTu>();
        TankController2 player = collision.GetComponent<TankController2>();

        Dogcollider dog = collision.GetComponent<Dogcollider>();

        SoldierMain soldier = collision.GetComponent<SoldierMain>();
       
        if(player != null)
        {
            player.TakeDamage(damage);
        }
        if(alliesTank != null)
        {

            alliesTank.TakeDamage(damage);
        }
        if (plane!=null)
        {
            plane.TakeDamage(damage);
        }
        if(soldier!=null)
        {
            soldier.TakeDamage(damage);
        }
        if (dog != null)
        {
            dog.TakeDamage(damage);
        }
        if(collision.tag == "enemy_controller")
        {
            collision.GetComponent<Assets.Scripts.Enemies.Enemy>().TakeDamage(damage);
        }
        


    }
    
}
