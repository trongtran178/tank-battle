using System.Collections;
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
        Enemy enemy= collision.GetComponent<Enemy>();
        TankController2 tank = collision.GetComponent<TankController2>();

        SoldierMain soldier = collision.GetComponent<SoldierMain>();
        if(enemy!=null)
        {
            enemy.TakeDamage(damage);
        }
        if(tank!=null)
        {
            
            tank.TakeDamage(damage);
        }
        if (plane!=null)
        {
            plane.TakeDamage(damage);
        }
        if(soldier!=null)
        {
            soldier.TakeDamage(damage);
        }


    }
    
}
