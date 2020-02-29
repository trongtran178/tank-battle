using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneCollider : MonoBehaviour
{

    public GameObject Tank;


    public float velx = 2f;
    public float vely = 0f;

    private GameObject projectile;

    public GameObject vfx_destroy;
    public GameObject location;
    //public int health;
  
    public Image healthyBar;
    public float maxHealth = 100f;
    public float PlaneHealth;

    private void Start()
    {
        PlaneHealth = maxHealth;
    }
    void Update()
    {
        healthyBar.fillAmount = PlaneHealth / maxHealth;
        if (PlaneHealth <=0)
            Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
 
        projectile = Instantiate(vfx_destroy, location.transform.position, location.transform.rotation) as GameObject;
        //projectile.transform.position = transform.position;
        //health -= damage;
      
        PlaneHealth -= damage;
       
        //PlaneHealthBar.PlaneHealth -= damage;
        projectile = null;
    }
}
