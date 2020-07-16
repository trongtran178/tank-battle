using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet3D : MonoBehaviourPun
{

    public float speed;

    private Vector2 force;
    public Rigidbody2D Rb;


    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Rb.velocity = new Vector2(speed, 0);

    }
    [PunRPC]
    public void destroy()
    {
        Destroy(this.gameObject);
    }
}
