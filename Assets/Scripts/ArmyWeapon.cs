using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyWeapon : MonoBehaviour
{

    private GameObject[] enemy;

    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectsWithTag("enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        SoldierMain soldier = collision.GetComponent<SoldierMain>();
        Enemy enemy1 = collision.GetComponent<Enemy>();

        Debug.Log(collision.ToString().Trim());
        if (collision.ToString().Trim().Equals("Soldier (UnityEngine.BoxCollider2D)"))
                soldier.TakeDamage(damage);
        if (collision.ToString() == "enemy (UnityEngine.PolygonCollider2D)")
            enemy1.TakeDamage(damage);
        
    }


}
