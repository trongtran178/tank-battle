using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBullet : MonoBehaviour
{

    public Image bullet;
    public float maxBullet;
    public float manaBullet;
    private bool flagMana=true;
  

    // Start is called before the first frame update
    void Start()
    {
        //bullet = GetComponent<Image>();
        manaBullet = maxBullet;
    }

    // Update is called once per frame
    void Update()
    {
        bullet.fillAmount = manaBullet / maxBullet;
        if (flagMana)
        {
            if (maxBullet == trajectoryScript.mana && trajectoryScript.flagForMana)
            {
                manaBullet = 1;
                trajectoryScript.mana = 0;
                flagMana = false;
            }
        }
        if (manaBullet < maxBullet)
        {
            manaBullet += 2f * Time.deltaTime;
            Debug.Log(manaBullet + ":" + trajectoryScript.block1);
        }
        if (manaBullet > maxBullet)
        {
            if (maxBullet == 10)
            {
                trajectoryScript.block1 = true;
                //flagMB1 = true;
            }
            if (maxBullet == 20)
            {
                trajectoryScript.block2 = true;
                //flagMB2 = true;
            }
            if (maxBullet == 30)
            {
                trajectoryScript.block3 = true;
                //flagMB3 = true;
            }


            flagMana = true;
            manaBullet = maxBullet;
            
            
        }
        
    }

   
}
