using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using UnityEngine;
using System.Linq;
namespace Assets.Scripts.Enemies
{
    public class EnemyBoss_Level2 : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;
        public GameObject child;

        private Animator animator; // include jump, idle, attack, fall back.
        private float currentHealth;
        private float minimumDistanceIndicatorBetweenAttackTarget = 15;
        private Rigidbody2D rigidBody2D;
        private float takeDamageRatio = .05f;
        private bool isDeath = false;
        private System.Random random = new System.Random();
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
            InvokeRepeating("HandleGenerateChild", 5.0f, 25.0f);
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
            if (attackTarget == null)
            {
                animator.Play("Idle");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong

            //if (animator.is("Mon_T_Attack")) return;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skill")) return;

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
            if (attackTarget == null) return;
            if (currentHealth <= 0) return;
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .05f);
        }

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
        }

        private void HandleAttack()
        {
            if (attackTarget == null || attackTarget.activeSelf == false) return;
            if (currentHealth <= 0) return;

            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget <= minimumDistanceIndicatorBetweenAttackTarget)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skill")) return;
                animator.Play("Attack");
                Invoke("AttackTargetTakeDamage", 1.2f);
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
                    attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(100);
                }
                else if (attackTarget.GetComponentInChildren<PlaneCollider>() != null)
                {
                    attackTarget.GetComponentInChildren<PlaneCollider>().TakeDamage(100);
                }
                else if (attackTarget.GetComponentInChildren<EnemyTu>() != null)
                {
                    attackTarget.GetComponentInChildren<EnemyTu>().TakeDamage(100);
                }
            }

            else
            {
                player.GetComponent<TankController2>()?.TakeDamage(30);
                // player.GetComponent<TankController3D>()?.TakeDamage(30);
            }

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
            animator.Play("Death");

            Invoke("DestroySelf", 8);
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
                if (isDeath == false)
                {
                    isDeath = true;
                    Death();
                    //foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
                    //{
                    //    if(enemy.GetComponentInChildren<Enemy>() != null)
                    //        enemy.GetComponentInChildren<Enemy>().Death();
                    //}
                }
            }
            else
            {
                // Take damage animator
                // animator.Play("Damage");
            }
        }


        // Special skill
        private void HandleGenerateChild()
        {
            if (attackTarget == null) return;
            if (isDeath || currentHealth <= 0) return;

            animator.Play("Skill");

            bool isBefore = false, isMiddle = false, isAfter = false;
            for (int i = 0; i < 3; i++)
            {
                Vector3 position = new Vector3();
                if (!isBefore)
                {
                    position.Set(self.transform.position.x - 8, self.transform.position.y, self.transform.position.z);
                    isBefore = !isBefore;
                }
                else if (!isMiddle)
                {
                    position.Set(self.transform.position.x, self.transform.position.y, self.transform.position.z);
                    isMiddle = !isMiddle;
                }
                else if (!isAfter)
                {

                    position.Set(self.transform.position.x + 8, self.transform.position.y, self.transform.position.z);
                    isAfter = !isAfter;
                }

                GameObject childObject = Instantiate(child, position, child.transform.rotation);
                childObject.SetActive(true);
                childObject.transform.localScale = new Vector3(2, 2, 2);
                EnemyFactory.enemies.Add(childObject);
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
            throw new System.NotImplementedException();
        }

        public override EnemyType GetEnemyType()
        {
            return EnemyType.BOSS_LEVEL_2;
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
