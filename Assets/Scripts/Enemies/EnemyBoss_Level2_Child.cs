using System;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyBoss_Level2_Child : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;
        public GameObject effectBuff;

        private Animator animator; // include walk, idle, attack
        private float currentHealth;
        private float minimumDistanceIndicatorBetweenAttackTarget = 7;
        private Rigidbody2D rigidBody2D;
        private double takeDamageRatio = .5;
        private bool isDeath = false;

        /// <summary>
        ///  If horizontalMove negative, enemy will moving in left side,
        ///  else if horizontalMove is positive, enemy will moving in right side,
        ///  else enemy will Idle
        /// </summary>
        private float horizontalMove = 0;

        // Su dung de xu ly di chuyen
        private Vector3 Velocity = Vector3.zero;

        void Awake()
        {
            animator = self.GetComponent<Animator>();
            rigidBody2D = self.GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        void Start()
        {
            IgnoreEnemies();
            attackTarget = FindAttackTarget();
            animator.Play("Idle");
            InvokeRepeating("HandleAttack", .1f, .1f);
            moveSpeed = 10;

            effectBuff.SetActive(true);
            effectBuff.GetComponentInChildren<ParticleSystem>().Play();
        }

        void Update()
        {
           
            if (currentHealth <= 0)
            {
                Death();
                return;
            }
            attackTarget = FindAttackTarget();
            Move();
        }

        private void Move()
        {
            if (currentHealth <= 0) return;
            if (attackTarget == null )
            {
                animator.Play("Idle");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

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
            if (distanceBetweenAttackTarget > minimumDistanceIndicatorBetweenAttackTarget)
            {
                CancelInvoke("HandleAttack");
                animator.Play("Walk");
            }
            else
            {
                InvokeRepeating("HandleAttack", .1f, .1f);
                horizontalMove = 0;
            }
        }

        private void FixedUpdate()
        {
            HandleMove();
        }

        private void HandleMove()
        {
            if (attackTarget == null || attackTarget.activeSelf == false || currentHealth <= 0) {
                CancelInvoke("HandleAttack");
                horizontalMove = 0;
                return;
            }
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .05f);
        }

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((float)(double)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
        }

        private void HandleAttack()
        {
            if (attackTarget == null || attackTarget.activeSelf == false) return;
            if (currentHealth <= 0) return;

            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget <= minimumDistanceIndicatorBetweenAttackTarget)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                animator.Play("Attack");
                Invoke("AttackTargetTakeDamage", .5f);
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
                if (attackTarget.GetComponentInChildren<Dogcollider>() != null)
                {
                    attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(20);
                }
                else if (attackTarget.GetComponentInChildren<PlaneCollider>() != null)
                {
                    attackTarget.GetComponentInChildren<PlaneCollider>().TakeDamage(20);
                }
                else if (attackTarget.GetComponentInChildren<EnemyTu>() != null)
                {
                    attackTarget.GetComponentInChildren<EnemyTu>().TakeDamage(20);
                }
            }

            else
                player.GetComponent<TankController2>().TakeDamage(20);

        }

        private bool IsFlip()
        {
            if (attackTarget.transform.position.x - transform.position.x > 0)
                return true;
            return false;
        }

        public override void Death()
        {
            CancelInvoke("HandleAttack");

            Invoke("DestroySelf", (float)0.5);
        }

        private void DestroySelf()
        {
            EnemyFactory.enemies.Remove(self);
            Destroy(self);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Bullet bullet = collider.GetComponent<Bullet>();

            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
        }

        public override void SetCurrentHealth(float currentHealth)
        {
            this.currentHealth = currentHealth;
        }

        public override float GetCurrentHealth()
        {
            return currentHealth;
        }

        public override void TakeDamage(float damage)
        {
            if (currentHealth <= 0 || isDeath) return;
            damage = (float)(damage * takeDamageRatio);
            currentHealth -= damage;

            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
            if (currentHealth <= 0)
            {
                isDeath = true;
                Destroy(self);
                //if (!isDeath)
                //{
                //    isDeath = true;
                //    foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
                //    {
                //        if (enemy.GetComponentInChildren<EnemyBoss_Level2_Child>() != null)
                //            enemy.GetComponentInChildren<Enemy>().Death();
                //    }
                //}
            }
            else
            {
                //currentHealth = 0;
                //currentHealthBar.transform.localScale = new Vector3(0, currentHealthBar.transform.localScale.y);
                //animation.Play("Mon_T_Dead");
                //Invoke("DestroySelf", 1);
                // Take damage animator
                // animator.Play("Damage");
            }
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
            // throw new System.NotImplementedException();
        }

        public override EnemyType GetEnemyType()
        {
            return EnemyType.BOSS_LEVEL_2_CHILD;
        }

        public override GameObject GetSelf()
        {
            return self;
        }

        public override bool IsShortRangeStrike()
        {
            return true;
        }
    }
}
