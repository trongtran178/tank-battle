using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierMain : MonoBehaviour
{


   
    private GameObject projectile;
    public GameObject vfx_destroy;
    public GameObject location;


    public Image healthyBar;
    public float maxHealthy = 100f;
    public float SoliderHealth;
    // Start is called before the first frame update
    void Start()
    {
        SoliderHealth = maxHealthy;
    }

    // Update is called once per frame
    void Update()
    {
        healthyBar.fillAmount = SoliderHealth / maxHealthy;
        if (SoliderHealth <= 0)
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {


        projectile = Instantiate(vfx_destroy, location.transform.position, location.transform.rotation) as GameObject;
        //projectile.transform.position = transform.position;


        SoliderHealth -= damage;
   
        projectile = null;
    }
}
