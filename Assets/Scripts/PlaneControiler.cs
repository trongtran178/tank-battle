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
        if (Tank != null)
        {
            kc = Tank.transform.position - transform.position;
        }
            float dolon = Mathf.Sqrt((kc.x * kc.x) + (kc.y * kc.y));
        
        checkflag = GameObject.Find("ColliderPlane");
        if(checkflag==null)
        {
            Destroy(gameObject);
        }
        vely = rb.velocity.y;
        rb.velocity = new Vector2(velx, vely);
    
            if ( dolon> distanceMax)
            {
                velx = -speed;

            }
            else if (dolon< distanceMin)
            {
                velx =speed;

            }
            else velx = 0;
      
      
    }

   
}
