
using UnityEngine.UI;
using UnityEngine;

public class Soldier_Health : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth;
    public GameObject player;

    public GameObject bloodSpurtEffect;
    // public GameObject healthBar;
    ParticleSystem hitParticles;
    bool isDead;
    void Awake()
    {
        hitParticles = GetComponentInChildren<ParticleSystem>();
        currentHealth = startingHealth;
        // healthBar.GetComponent<HealthBar>().currentHealth = startingHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
 
        if (other.name == player.name)
        {
            Instantiate(bloodSpurtEffect, transform.position, Quaternion.identity);
           
        }
    }

    public void TakeDamage(int amount, Vector2 hitPoint)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        hitParticles.transform.position = hitPoint;

        hitParticles.Play();

        if (currentHealth <= 0)
            Death();
    }

    void Death()
    {
        isDead = true;
        Destroy(gameObject, 2f);
    }


}
