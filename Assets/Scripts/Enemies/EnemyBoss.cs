using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyBoss : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;


        private Animator animator; // include jump, idle, attack, fall back.
        private float currentHealth;
        private float minimumDistanceIndicatorBetweenAttackTarget = 20;
        private bool isBumpingHealth = false;
        private Rigidbody2D rigidBody2D;
        private int countBumpHealh = 0;
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
            
            currentHealth = health;
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
        }

        void Start()
        {
            attackTarget = FindAttackTarget();
            InvokeRepeating("HandleAttack", .1f, .1f);
            // HandleBumpBloodForTeammates();
            Debug.Log(attackTarget);
        }

        void Update()
        {
            attackTarget = FindAttackTarget();
            Debug.Log(attackTarget);
            Move();
            HandleBumpBloodForTeammates();
        }

        private void Move()
        {
            if (attackTarget == null)
            {
                animator.Play("idle_normal");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong

            //if (animator.is("Mon_T_Attack")) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_short_001")) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("buff_health")) return;

            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget > minimumDistanceIndicatorBetweenAttackTarget)
            {
                CancelInvoke("HandleAttack");
                animator.Play("move_forward");
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

        private void HandleAttack()
        {
            if (player == null || player.activeSelf == false || attackTarget == null) return;
            if (currentHealth <= 0) return;

            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget <= minimumDistanceIndicatorBetweenAttackTarget)
            {
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack_short_001")) return;
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("buff_health")) return;
                animator.Play("attack_short_001");
                //animator.Play("buff_health");
                //Invoke("AwaitPlayerTakeDamage", .3f);
                /////////////////////////////////////////
                /////////// IMPORTANT CODE //////////////
                /////////// SET SPEED OF ANIMATION //////
                /////////////////////////////////////////
                // self.GetComponent<Animation>()["Mon_T_Attack"].speed = (float) 2.3;
            }
        }

        private bool IsFlip()
        {
            if (attackTarget.transform.position.x - transform.position.x > 0)
                return true;
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
                if (currentHealth <= 0) return;
                currentHealth -= 30;

                currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
                if (currentHealth <= 0)
                {
                    CancelInvoke("HandleAttack");
                    animator.Play("dead");
                    //Invoke("Death", 8);
                }
                else
                {
                    animator.Play("damaged_001");
                    //CancelInvoke("HandleAttack");
                    //InvokeRepeating("HandleAttack", 3.0f, .1f);
                }
            }
        }

        private void HandleBumpBloodForTeammates()
        {
            // Boss chi duoc bom mau toi da 10 lan
            if (countBumpHealh >= 10) return;
            Dictionary<GameObject, float> enemyDictionary = new Dictionary<GameObject, float>();
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                if (!enemy.Equals(self) && !enemyDictionary.ContainsKey(enemy))
                {
                    float distanceBetweenBoss = Vector2.Distance(self.transform.position, enemy.transform.position);
                    enemyDictionary.Add(enemy, distanceBetweenBoss);
                }
            }

            GameObject enemyNeedBumpHealth = enemyDictionary.FirstOrDefault(x =>
                                            (x.Value <= 15 && x.Key.GetComponentInChildren<Enemy>().GetCurrentHealth() <= 30
                                            )).Key;
            if (enemyNeedBumpHealth != null)
            {
                countBumpHealh++;
                Debug.Log(countBumpHealh);
                CancelInvoke("HandleAttack");
                animator.Play("buff_health");
                enemyNeedBumpHealth.GetComponentInChildren<Enemy>().ReceiveHealthBumpFromBoss();
                InvokeRepeating("HandleAttack", 2.0f, .1f);
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



    }
}
