using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : MonoBehaviour
{
    private const double g = 9.8;
    private GameObject bullet;
    public GameObject bulletMain;
    public GameObject tank;
    public float velx;
    public float vely;
    public float goc;

    public Vector3 kc;
    private Vector2 shotForce;
    public float v=0;

    public float doLonKc=0;
    bool flag=false;
    public float timeGun = 3.0f;
    public bool flagGun;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (tank != null)
        {

            kc = tank.transform.position - transform.position;

            doLonKc = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));
            v = Mathf.Sqrt((float)((doLonKc * g) / Mathf.Sin(2 * goc)));

            velx = v * Mathf.Cos(goc);

            vely = v * Mathf.Sin(goc);

            shotForce = new Vector2(-velx, vely);
            //if (trajectoryScript.flagShoot == false)
            //{
                timeGun -= Time.deltaTime;
                if (timeGun <= 0)
                {
                    //if (Input.GetKeyDown("space"))
                    //{
                        if (flag == false)
                        {
                            flag = true;
                            bullet = Instantiate(bulletMain, transform.position, transform.rotation) as GameObject;

                            bullet.GetComponent<Rigidbody2D>().isKinematic = true;
                            //projectile1.SetActive(false);
                        }
                    //}


                    //if (Input.GetKeyUp("space"))
                    //{
                        if (bullet != null)
                        {
                            bullet.SetActive(true);
                            bullet.GetComponent<Rigidbody2D>().isKinematic = false;
                            bullet.GetComponent<Rigidbody2D>().AddForce(shotForce, ForceMode2D.Impulse);
                            //Debug.Log(bullet.transform.position);
                            flag = false;
                            bullet = null;
                            trajectoryScript.flagShoot = true;
                        }
                    //flag = false;
                    //bullet = null;
                    //trajectoryScript.flagShoot = true;
                    //}

                    timeGun = 3f;
                }
            //}
        }

    }
}
