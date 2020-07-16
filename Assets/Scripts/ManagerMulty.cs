using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ManagerMulty : MonoBehaviour
{
    public GameObject player;
    public GameObject SpawPoint;
    // Start is called before the first frame update
    void Start()
    {
        SpawPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(8, 8);
    }

    void SpawPlayer()
    {
        PhotonNetwork.Instantiate(player.name, SpawPoint.transform.position, player.transform.rotation);
    }
}
