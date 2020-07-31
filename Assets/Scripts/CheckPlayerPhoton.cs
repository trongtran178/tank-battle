using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerPhoton : MonoBehaviour
{
    public GameObject EnemyHouse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] tanks= GameObject.FindGameObjectsWithTag("player");
        if (tanks.Length > 1)
            EnemyHouse.SetActive(true);

    }
}
