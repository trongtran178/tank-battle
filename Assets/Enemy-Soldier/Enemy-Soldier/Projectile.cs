using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;

    public GameObject explosion;

    public GameObject player;


    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        // edit tag, hard code
        player = GameObject.FindGameObjectWithTag("player2");    
        target = new Vector2(player.transform.position.x, player.transform.position.y);
    }   
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(transform.position.x == target.x  && transform.position.y  == target.y)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            DestroyProjectile();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {       
        if(other.CompareTag("player2"))
        {
            Debug.Log("asdsadasddasdsa");
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            
            DestroyProjectile();
            
        }
    }
   
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
} 
