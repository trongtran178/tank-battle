using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArmyWeapon : MonoBehaviour
{

    private GameObject[] enemy;

    public int damage = 1;
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
        Debug.Log(collision.name);
        SoldierMain soldier = collision.GetComponent<SoldierMain>();
        Enemy enemy1 = collision.GetComponent<Enemy>();
        if (collision.ToString().Trim().Equals("Soldier (UnityEngine.BoxCollider2D)"))
            soldier.TakeDamage(damage);
        if (collision.ToString() == "enemy (UnityEngine.PolygonCollider2D)")
            enemy1.TakeDamage(damage);
        if (collision.name.Equals("Enemy_Controller"))
            collision.GetComponent<Assets.Scripts.Enemies.Enemy>().TakeDamage(damage);
        if (collision.name.Equals("Enemy_House_Controller"))
            collision.GetComponent<Assets.Scripts.Enemies.Enemy>().TakeDamage(damage);
    }
}
