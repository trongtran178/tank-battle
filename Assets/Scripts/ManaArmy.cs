using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaArmy : MonoBehaviour
{

    public Image army;
    public float maxArmy;
    public float manaArmy;
    private bool flagMana = true;

    private GameObject projectile;
    // Rename projectileGame1 -> alliesObject
    public GameObject alliesObject; // tank, dog, plane
    public Vector3 vector3;

    public GameObject location;
    public KeyCode key;
    public bool isLock = false;
    // Start is called before the first frame update
    void Start()
    {
        manaArmy = maxArmy;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLock) return;
        army.fillAmount = manaArmy  / maxArmy;


        
        if (manaArmy < maxArmy)
        {
            manaArmy += 2f * Time.deltaTime;

        }
        else
        {
            manaArmy = maxArmy;
            if (Input.GetKey(key))
            {
                manaArmy = 0;
                
                //vector3.z = 0;
                //vector3.y = 90;
                //vector3.x = 0;
                projectile = Instantiate(alliesObject, location.transform.position,Quaternion.Euler(vector3));
                projectile.SetActive(true);
            }
            
            projectile = null;
        }

    }
}
