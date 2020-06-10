using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class FrogEnemy : Enemy
    {
        public GameObject currentHealthBar;
        public GameObject effectBuff;
        public GameObject self;

        private float currentHealth;
        private Rigidbody2D rigidBody2D;
        private new Animation animation;
        /// <summary>
        ///  If horizontalMove negative, enemy will moving in left side,
        ///  else if horizontalMove is positive, enemy will moving in right side,
        ///  else enemy will Idle
        /// </summary>
        private float horizontalMove = 0;

        // Su dung de xu ly di chuyen
        private Vector3 Velocity = Vector3.zero;
        // private GameObject attackTarget;

        void Awake()
        {
            animation = self.GetComponent<Animation>();
            attackTarget = FindAttackTarget();
            currentHealth = health;
            effectBuff.SetActive(false);
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
        }

        void Start()
        {
            InvokeRepeating("HandleAttack", .1f, .1f);
        }

        void Update()
        {
            attackTarget = FindAttackTarget();
            Move();
        }

        private void Move()
        {
            if(attackTarget == null)
            {
                animation.Play("Mon_T_Jump");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong
            if (self.GetComponent<Animation>().IsPlaying("Mon_T_Attack")) return;

            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget >= 7)
            {
                CancelInvoke("Attack");
                animation.Play("Mon_T_Run");
                if (IsFlip())
                {
                    self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, 90, self.transform.eulerAngles.z);
                    horizontalMove = 1 * moveSpeed;
                }
                else
                {
                    self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, -90, self.transform.eulerAngles.z);
                    horizontalMove = -1 * moveSpeed;
                }
            }
            else
            {
                horizontalMove = 0;
            }
        }

        private void FixedUpdate()
        {
            HandleMove();
        }

        private void HandleMove()
        {
            if (attackTarget == null) return;
            if (currentHealth <= 0) return;
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .05f);
        }

        private void HandleAttack()
        {
            if (player == null || player.activeSelf == false || attackTarget == null) return;
            if (currentHealth <= 0) return;
            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);

            if (animation.IsPlaying("Mon_T_Attack")) return;
            else if (distanceBetweenAttackTarget < 7)
            {
                animation.Play("Mon_T_Attack");
                //Invoke("AwaitPlayerTakeDamage", .3f);
                /////////////////////////////////////////
                /////////// IMPORTANT CODE //////////////
                /////////// SET SPEED OF ANIMATION //////
                /////////////////////////////////////////
                // self.GetComponent<Animation>()["Mon_T_Attack"].speed = (float) 2.3;
            }
        }

        private void AwaitPlayerTakeDamage()
        {
            player.GetComponent<TankController2>().TakeDamage(20);
        }

        private bool IsFlip()
        {
            if(attackTarget.transform.position.x - transform.position.x > 0)
            {
                return true;
            }
            return false;
        }

        private void Death()
        {
            Destroy(self);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Bullet bullet = collider.GetComponent<Bullet>();
          
            if (bullet != null)
            {
                currentHealth -= bullet.damage;
                currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
                if (currentHealth <= 0)
                {
                    animation.Play("Mon_T_Dead");
                    Invoke("Death", 2);
                }
            }
        }

        private void ReceiveBuff()
        {
            currentHealth += 10;
            effectBuff.SetActive(true);
        }


        public override void SetCurrentHealth(float currentHealth)
        {
            this.currentHealth = currentHealth;
        }
        public override float GetCurrentHealth()
        {
            return currentHealth;
        }

        public override void UpgrageLevelCorrespondToPhase(Phase phase)
        {
            throw new System.NotImplementedException();
        }

        public override void Instantiate()
        {
            throw new System.NotImplementedException();
        }

        public override void ReceiveHealthBumpFromBoss()
        {
            currentHealth += 30;
            currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
            Debug.Log("166 - co avo daay");
            effectBuff.SetActive(true);
            effectBuff.GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
