using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class MechsRobotEnemy : Enemy
    {
        public float minimumDistanceIndicatorBetweenPlayer = 24;

        public GameObject self;
        public GameObject currentHealthBar;
        public GameObject weaponLeft;
        public GameObject weaponRight;
        public GameObject effectTakeDamage;
        public GameObject effectDestroy;


        private Animator animator; // include jump, idle, attack, fall back.
        private Animator weaponLeftAnimator;
        private Animator weaponRightAnimator;

        private float currentHealth;
        private float horizontalMove = 0;

        private Rigidbody2D rigidBody2D;

        private GameObject player;
        private GameObject player_body;
       

        // Su dung de xu ly di chuyen
        private Vector3 Velocity = Vector3.zero;

        void Awake()
        {
            currentHealth = health;
            rigidBody2D = GetComponentInParent<Rigidbody2D>();

            player = GameObject.FindGameObjectWithTag("player");
            player_body = GameObject.FindGameObjectWithTag("player_body");

            animator = GetComponentInParent<Animator>();
            weaponLeftAnimator = weaponLeft.GetComponent<Animator>();
            weaponRightAnimator = weaponRight.GetComponent<Animator>();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            animator.Play("Walk");
            weaponLeftAnimator.Play("Idle");
            weaponRightAnimator.Play("Idle");
        }

        // Update is called once per frame
        void Update()
        {
            if(currentHealth > 0)
            {
                Move();
            }
        }

        private void Move()
        {
            if (player == null || player.activeSelf == false)
            {
                self.GetComponent<Animation>().Play("Jump_jetpack");
                return;
            }
            // Neu enemy het mau thi khong the di chuyen duoc
            else if (currentHealth <= 0) return;
            else if(player.activeSelf)
            {
                float distanceBetweenPlayer = Vector2.Distance(player.transform.position, self.transform.position);
           
                if (distanceBetweenPlayer >= (minimumDistanceIndicatorBetweenPlayer - 8) && Vector2.Distance(player.transform.position, self.transform.position) <= (minimumDistanceIndicatorBetweenPlayer + 8))
                {
                    horizontalMove = 0;
                    animator.Play("Idle");
                    weaponLeftAnimator.Play("Shoot");
                    weaponRightAnimator.Play("Shoot");
                }
                else if (distanceBetweenPlayer > minimumDistanceIndicatorBetweenPlayer + 8)
                {
                    horizontalMove = -1 * moveSpeed;
                    animator.Play("Walk");
                    weaponLeftAnimator.Play("Idle");
                    weaponRightAnimator.Play("Idle");
                }
                else if (distanceBetweenPlayer < minimumDistanceIndicatorBetweenPlayer - 8)
                {  
                    horizontalMove = moveSpeed;
                    animator.Play("Walk");
                    weaponLeftAnimator.Play("Idle");
                    weaponRightAnimator.Play("Idle");
                }
            }
        }

        private void Attack()
        {
            //if (!self.activeSelf || self == null) return;
            //Quaternion quaternion = new Quaternion(20, 0, self.transform.rotation.z, self.transform.rotation.w);
            //Instantiate(vfx_projectile, firePoint.transform.position, Quaternion.Euler((float)getAttackCorner(), firePoint.rotation.y, firePoint.rotation.z));
        }

        private void FixedUpdate()
        {
            HandleMove();
        }

        private void HandleMove()
        {
            if (currentHealth <= 0) return;
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .05f);
        }

        private bool IsFlip()
        {
            if (player_body.transform.position.x - transform.position.x > 0)
            {
                return true;
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(FreezeRotation(.1f));
                currentHealth -= bullet.damage;
                currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
                if (currentHealth <= 0)
                {
                    animator.Play("Fall_back");
                    weaponLeftAnimator.Play("Idle");
                    weaponRightAnimator.Play("Idle");
                    Invoke("Death", 2);
                }
                
            }
        }

        private void Death()
        {
            Destroy(self);
        }

        private IEnumerator FreezeRotation(float time)
        {
            yield return new WaitForSeconds(time);
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private double getAttackCorner()
        {
            float x = Mathf.Abs(Mathf.Abs(self.transform.position.x) - Mathf.Abs(player.transform.position.x));
            float y = Mathf.Abs(Mathf.Abs(self.transform.position.y) - Mathf.Abs(player.transform.position.y));
            return (Mathf.Atan(y / x) * (180 / 3.14));
        }


        public override void UpgrageLevelCorrespondToPhase(Phase phase)
        {
            throw new System.NotImplementedException();
        }

        public override void Instantiate()
        {
            throw new System.NotImplementedException();
        }

    }
}