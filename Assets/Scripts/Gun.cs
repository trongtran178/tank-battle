﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float offset=0;
    public GameObject projectile;
    public Transform shotPoint;
    private float timeBtwShots;
    public float startTimeBtwShots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotZ = Mathf.Atan2(diffrence.y, diffrence.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);




        //if (timeBtwShots <= 0)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Instantiate(projectile, transform.position, transform.rotation);
        //        timeBtwShots = startTimeBtwShots;
        //    }
        //    else timeBtwShots -= Time.deltaTime;
        //}
    }
}
