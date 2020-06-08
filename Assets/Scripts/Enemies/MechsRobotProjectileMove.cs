using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechsRobotProjectileMove : MonoBehaviour
{
    public GameObject self;
    public GameObject explosion;
    public GameObject player;
    public GameObject firePoint;
    public float speed;

    private Vector2 target;
    private GameObject player_body;
    private float initializationTime;
    // Start is called before the first frame update
    void Start()
    {
           // edit tag, hard code
        player = GameObject.FindGameObjectWithTag("player");
        player_body = GameObject.FindGameObjectWithTag("player_body");
        
        if (player) {
            target = new Vector2(-1000.0f, firePoint.transform.position.y);
        }
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (!self) return;
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (timeSinceInitialization >= 4)
        {
            DestroyProjectile();
            return;
        }

        self.transform.position = Vector2.MoveTowards(self.transform.position, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.CompareTag("player") || collision.name == "GroundGrass")
        {
            // player.GetComponent<TankController2>().TakeDamage(20);
            DestroyProjectile();
        }   
        else
        {

        }
    }

    void DestroyProjectile()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(self);
    }
}
