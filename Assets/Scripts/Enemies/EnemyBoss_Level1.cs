using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.Enemies
{

    public class EnemyBoss_Level1 : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;

        private Animator animator; // include jump, idle, attack, fall back.
        private float currentHealth;
        private float minimumDistanceIndicatorBetweenAttackTarget = 15;
        private Rigidbody2D rigidBody2D;
        private double takeDamageRatio = .3;
        private bool isDeath = false;
        private GameObject enemyNeedBumpHealth;
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

            currentHealth = maxHealth;
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
        }

        void Start()
        {
            IgnoreEnemies();
            attackTarget = FindAttackTarget();
            InvokeRepeating("HandleAttack", .1f, .1f);
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
            HandleBumpBloodForTeammates();
        }

        private void Move()
        {
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                CancelInvoke("HandleAttack");
                animator.Play("idle_normal");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong

            //if (animator.is("Mon_T_Attack")) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_short_001")) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("buff_health")) return;
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
                animator.Play("move_forward");
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
            if (attackTarget == null || attackTarget.activeSelf == false) return;
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
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_short_001")) return;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("buff_health")) return;
                animator.Play("attack_short_001");
                Invoke("AttackTargetTakeDamage", .8f);
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
                    attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(30);
                }
                else if (attackTarget.GetComponentInChildren<PlaneCollider>() != null)
                {
                    attackTarget.GetComponentInChildren<PlaneCollider>().TakeDamage(30);
                }
                else if (attackTarget.GetComponentInChildren<EnemyTu>() != null)
                {
                    attackTarget.GetComponentInChildren<EnemyTu>().TakeDamage(30);
                }
            }
                
            else
            {
                player.GetComponent<TankController2>()?.TakeDamage(30);
                player.GetComponent<TankController3D>()?.TakeDamage(30);

            }


        }

        private void HandleBumpBloodForTeammates()
        {
            Dictionary<GameObject, float> enemyDictionary = new Dictionary<GameObject, float>();
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                if (!enemy.Equals(self) && !enemyDictionary.ContainsKey(enemy))
                {
                    float distanceBetweenBoss = Vector2.Distance(self.transform.position, enemy.transform.position);
                    enemyDictionary.Add(enemy, distanceBetweenBoss);
                }
            }

            enemyNeedBumpHealth = enemyDictionary.FirstOrDefault(x =>
                                           (x.Key != null
                                           && x.Value <= 10
                                           && x.Key.GetComponentInChildren<Enemy>() != null
                                           && x.Key.GetComponentInChildren<Enemy>().GetCurrentHealth() <= 30
                                           && x.Key.GetComponentInChildren<Enemy>().GetCurrentHealth() > 0
                                           )).Key;
            if (enemyNeedBumpHealth != null)
            {
                CancelInvoke("HandleAttack");
                animator.Play("buff_health");
                HandleBumpBloodForTeammatesTemp();
            }
        }

        void HandleBumpBloodForTeammatesTemp()
        {
            if (enemyNeedBumpHealth.GetComponentInChildren<Enemy>().GetCurrentHealth() <= 30)
            {
                enemyNeedBumpHealth.GetComponentInChildren<Enemy>().ReceiveHealthBumpFromBoss();
            }
            InvokeRepeating("HandleAttack", 2.0f, .1f);
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
            animator.Play("dead");

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
            damage = (float) (damage * takeDamageRatio);
            currentHealth -= damage;

            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
            if (currentHealth <= 0)
            {
                if (!isDeath)
                {
                    isDeath = true;
                    Death();
                    //foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
                    //{
                    //    enemy.GetComponentInChildren<Enemy>()?.Death();
                    //}
                }
            }
            else
            {
                animator.Play("damaged_001");
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

        }

        public override EnemyType GetEnemyType()
        {
            return EnemyType.BOSS_LEVEL_1;
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
