using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechsRobotProjectileMove : MonoBehaviour
{

    public GameObject self;
    public GameObject explosion;
    public GameObject player;
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
        if(player)
            target = new Vector2(player_body.transform.position.x, player_body.transform.position.y);
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        if (timeSinceInitialization >= 3)
        {
            Instantiate(explosion, transform.position, transform.rotation);

            DestroyProjectile();
        }
        self.transform.position = Vector2.MoveTowards(self.transform.position, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            player.GetComponent<TankController2>().TakeDamage(20);
            Instantiate(explosion, transform.position, transform.rotation);
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(self);
    }
}
