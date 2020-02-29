using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPlane : MonoBehaviour
{

    private GameObject bullet;
    public GameObject bulletMain;
    public GameObject tank;
    public float velx;
    public float vely;
    private Vector2 target;
    public float speed;
    bool flag = false;
    public float timeGunPlane = 3.0f;
    private Vector2 shotForce;

    public Vector3 kc;
    public float doLonKcPlane;
    // Start is called before the first frame update
    void Start()
    {
        target = new Vector2(tank.transform.position.x, tank.transform.position.y);
       // shotForce = new Vector2(-10, -20);
    }

    // Update is called once per frame
    void Update()
    {
        if (tank!=null)
        {
            kc = tank.transform.position - transform.position;
            doLonKcPlane= Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));
            shotForce = new Vector2(kc.x, kc.y);
            timeGunPlane -= Time.deltaTime;
            if (timeGunPlane <= 0)
            {
                if (flag == false)
                {
                    flag = true;
                    bullet = Instantiate(bulletMain, transform.position, transform.rotation) as GameObject;

                    bullet.GetComponent<Rigidbody2D>().isKinematic = true;
                    //projectile1.SetActive(false);
                }
                if (bullet != null)
                {
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody2D>().isKinematic = false;
                    bullet.GetComponent<Rigidbody2D>().AddForce(shotForce, ForceMode2D.Impulse);
                    //bullet.transform.position = Vector2.MoveTowards(bullet.transform.position, target, speed * Time.deltaTime);
                    //Debug.Log(bullet.transform.position);
                    flag = false;
                    bullet = null;
                    //trajectoryScript.flagShoot = true;
                }
                timeGunPlane = 3f;
            }
            
        }
    }
    public float GetKC()
    {
        return doLonKcPlane;
    }
}
