using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Assets.Scripts.Enemy;

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
            currentHealth = maxHealth;
            effectBuff.SetActive(false);
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
        }

        void Start()
        {
            InvokeRepeating("HandleAttack", .1f, .1f);
        }

        void Update()
        {
            if(currentHealth <= 0)
            {
                Death();
                return;
            }
            attackTarget = FindAttackTarget();
            Move();
        }

        private void Move()
        {
            if (attackTarget == null)
            {
                animation.Play("Mon_T_Jump");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong
            if (self.GetComponent<Animation>().IsPlaying("Mon_T_Attack")) return;

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


            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget >= 7)
            {
                CancelInvoke("Attack");
                animation.Play("Mon_T_Run");

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

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
        }

        private void HandleAttack()
        {
            //if (player == null || player.activeSelf == false || attackTarget == null) return;
            if (attackTarget == null || !attackTarget.activeSelf) return;
            if (currentHealth <= 0) return;
            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);

            if (animation.IsPlaying("Mon_T_Attack")) return;
            else if (distanceBetweenAttackTarget < 7)
            {
                animation.Play("Mon_T_Attack");
                Invoke("AttackTargetTakeDamage", 0.3f);
                /////////////////////////////////////////
                /////////// IMPORTANT CODE //////////////
                /////////// SET SPEED OF ANIMATION //////
                /////////////////////////////////////////
                // self.GetComponent<Animation>()["Mon_T_Attack"].speed = (float) 2.3;
            }
        }

        private void AttackTargetTakeDamage()
        {
            if (attackTarget == null) return;
            if (attackTarget.tag.Equals("allies"))
            {
                attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(20);
            }
            else
            {
                player.GetComponent<TankController2>().TakeDamage(20);
            }
        }

        private bool IsFlip()
        {
            if (attackTarget.transform.position.x > transform.position.x)
            {
                return true;
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Bullet bullet = collider.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
        }

        public override void TakeDamage(int damage)
        {
            Debug.Log(damage);
            currentHealth -= damage;
            currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
            if (currentHealth <= 0)
            {
                Death();
            }
        }

        public override void Death()
        {
            currentHealth = 0;
            currentHealthBar.transform.localScale = new Vector3(0, currentHealthBar.transform.localScale.y);
            animation.Play("Mon_T_Dead");
            Invoke("DestroySelf", 2);
            // Destroy(self);
        }

        private void DestroySelf()
        {
            EnemyFactory.enemies.Remove(self);
            Destroy(self);
        }

        //private void TakeDamage()
        //{

        //}

      
        public override void SetCurrentHealth(float currentHealth)
        {
            this.currentHealth = currentHealth;
        }
        public override float GetCurrentHealth()
        {
            return currentHealth;
        }

        public override EnemyType GetEnemyType()
        {
            return EnemyType.FROG;
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
            Invoke("HandleReceiveHealthBumpFromBoss", 3.0f);
        }

        private void HandleReceiveHealthBumpFromBoss()
        {
            if (currentHealth <= 30 && currentHealth > 0)
            {
                currentHealth += 30;
                currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
                effectBuff.SetActive(true);
                effectBuff.GetComponentInChildren<ParticleSystem>().Play();
            }
        }

        public override GameObject GetSelf()
        {
            return self;
        }
    }
}

