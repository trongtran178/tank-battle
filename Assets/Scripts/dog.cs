using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dog : MonoBehaviour
{

    private Animator ami;
    private Rigidbody2D rb;
    private float velx = 5;
    private float vely = 0;
    private Vector3 kc;


    private GameObject[] enemy;

    private GameObject checkflag;

    // Start is called before the first frame update
    void Start()
    {
        ami = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();



    }

    // Update is called once per frame
    void Update()
    {

        Physics2D.IgnoreLayerCollision(9, 9);
        vely = rb.velocity.y;
        rb.velocity = new Vector2(velx, vely);
        checkflag = GameObject.Find("colliderDog");
        if (checkflag == null)
        {
            Destroy(gameObject);
        }
        if (velx > 0)
        {
            ami.SetBool("isWalk", true);
            //ami.SetBool("isAttack", true);
        }

        GameObject k = findEnemy();
        if(k!=null)
        {
            kc = k.transform.position - gameObject.transform.position;
            float doLonKc = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));

            //Debug.Log(doLonKc);
            if (doLonKc < 10)
            {
                //Debug.Log("da vao");
                velx = 0;
                ami.SetBool("isWalk", false);
                ami.SetBool("isAttack", true);
            }
            else
            {
                velx = 5;
                ami.SetBool("isWalk", true);
                ami.SetBool("isAttack", false);
            }
        }

        //foreach (GameObject e in enemy)
        //{
        //    if (e != null)
        //    {
        //        kc = e.transform.position - gameObject.transform.position;
        //        float doLonKc = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));

        //        //Debug.Log(doLonKc);
        //        if (doLonKc < 10)
        //        {
        //            //Debug.Log("da vao");
        //            velx = 0;
        //            ami.SetBool("isWalk", false);
        //            ami.SetBool("isAttack", true);
        //        }
        //        else
        //        {
        //            velx = 5;
        //            ami.SetBool("isWalk", true);
        //            ami.SetBool("isAttack", false);
        //        }
        //    }

        //}

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
