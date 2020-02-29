using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_Shot_Player : MonoBehaviour
{

    public Transform SoldierEnemy;

    public GameObject projectile;

    public float timeBtwShots;

    public float startTimeBtwShots;

    public Animator animator;

    public Transform player;
    public Transform firePoint;


    public float distanceShot;
        
    private bool isObstacles = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     
        // ColliderDistance2D.

        // Debug.Log(Physics.Linecast(, player.position));
      
        if (!isObstacles)
        {
            if (Vector2.Distance(player.position, firePoint.transform.position) <= distanceShot)
            {
                if (timeBtwShots <= 0)
                {
                    animator.SetBool("isAttack", true);

                    Instantiate(projectile, firePoint.position, Quaternion.identity);
                   
                    timeBtwShots = startTimeBtwShots;  
                }
                else
                {
                    animator.SetBool("isAttack", false);
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }

    private void checkObstacle()
    {
        
    }


}
