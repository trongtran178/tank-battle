using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControiler : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    public GameObject Tank;

    private GunPlane gunPlane;
    private float velx;
    private float vely;
    public float speed;
    private Vector3 kc;
    public float distanceMax;
    public float distanceMin;
    private GameObject[] enemy;
    private GameObject checkflag;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        //gunPlane = new GunPlane();
    }
                        
    // Update is called once per frame
    void Update()
    {
        //if (Tank != null)
        //{
        //    kc = Tank.transform.position - transform.position;
        //}
        //    float dolon = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));
        
        checkflag = GameObject.Find("ColliderPlane");
        if(checkflag==null)
        {
            Destroy(gameObject);
        }
        vely = rb.velocity.y;
        rb.velocity = new Vector2(velx, vely);

        GameObject k = findEnemy();
        if (k != null)
        {
            kc = k.transform.position - gameObject.transform.position;
            float doLonKc = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));
            if (doLonKc < 50)
            {
                velx = 0;
            }
            else velx = speed;
        }

        //if ( dolon> distanceMax)
        //{
        //    velx = -speed;

        //}
        //else if (dolon< distanceMin)
        //{
        //    velx =speed;

        //}
        //else velx = 0;


    }

    GameObject findEnemy()
    {
        enemy = GameObject.FindGameObjectsWithTag("enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject e in enemy)
        {
            Vector3 diff = e.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = e;
                distance = curDistance;

            }
        }
        return closest;


    }


}
