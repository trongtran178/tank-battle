using UnityEngine;

namespace Assets.Scripts.Enemies
{ 
    public class EnemyBoss_Level2_Child : Enemy
    {
        public GameObject currentHealthBar;
        public GameObject effectBuff;
        public GameObject self;
        public GameObject triggerEvent;

        private TriggerEvent triggerEventScript;
        private float currentHealth;
        private float takeDamageRatio = 1.0f;
        private Rigidbody2D rigidBody2D;
        private Animator animator;
        
        /// <summary>
        ///  If horizontalMove negative, enemy will moving in left side,
        ///  else if horizontalMove is positive, enemy will moving in right side,
        ///  else enemy will Idle
        /// </summary>
        private float horizontalMove = 0f;

        // Su dung de xu ly di chuyen
        private Vector3 Velocity = Vector3.zero;

        void Start()
        {
            animator = self.GetComponent<Animator>();
            currentHealth = maxHealth;
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
            IgnoreEnemies();
            InvokeRepeating("HandleAttack", .1f, .1f);
            moveSpeed = 10.0f;
            triggerEventScript = triggerEvent.GetComponent<TriggerEvent>();
        }

        void Update()
        {
            if (currentHealth <= 0)
            {
                Death();
                return;
            }
            attackTarget = FindAttackTarget();
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                horizontalMove = 0f;
                return;
            }
            else if (currentHealth > 0 && attackTarget != null && attackTarget.activeSelf) Move();
            Debug.Log(triggerEventScript == null);
            if (triggerEventScript.IsWin == true || triggerEventScript.IsLose == true)
            {
                EnemyFactory.enemies.Remove(self);
                Destroy(self);
            }
           
        }



        void Move()
        {
            Debug.Log(horizontalMove);
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                animator.Play("Idle");
                horizontalMove = 0f;
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;

            if (IsFlip())
            {
                if (attackTarget == null) {
                    
                    horizontalMove = 0f;
                    return;
                }
                
                self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, 90, self.transform.eulerAngles.z);
                horizontalMove = 1f * moveSpeed;
            }
            else
            {
                if (attackTarget == null)
                {
                    horizontalMove = 0f;
                    return;
                }
                self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, -90, self.transform.eulerAngles.z);
                horizontalMove = -1f * moveSpeed;
            }


            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
            if (distanceBetweenAttackTarget >= 10)
            {
                CancelInvoke("Attack");
                animator.Play("Walk");

            }
            else
            {
                horizontalMove = 0f;
            }
        }

        private void FixedUpdate()
        {
            if (attackTarget == null)
            {
                horizontalMove = 0f;
                return;
            }
            HandleMove();
        }

        private void HandleMove()
        {
            if (attackTarget == null || attackTarget.activeSelf == false) {
                horizontalMove = 0f;
                return;
            };
            if (currentHealth <= 0) {
                horizontalMove = 0f;
                return;
            }
           
            Vector3 targetVelocity = new Vector2(horizontalMove * 10.0f * Time.deltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .01f);
        }

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
        }

        private void HandleAttack()
        {
            if (attackTarget == null || !attackTarget.activeSelf || GameObject.FindGameObjectsWithTag("player").Length <= 0) {
                horizontalMove = 0f;
                animator.Play("Idle");
                return;
            }
            
            if (currentHealth <= 0) return;
            float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                horizontalMove = 0f;
                return;
            } 
            else if (distanceBetweenAttackTarget < 10)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    horizontalMove = 0f;
                    return;
                }
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
            if (attackTarget == null) {
                horizontalMove = 0f;
                return;
            }
           
            if (attackTarget.tag.Equals("allies") || attackTarget.tag.Equals("allies_collider"))
            {
                if (attackTarget.GetComponentInChildren<Dogcollider>() != null)
                {
                    attackTarget.GetComponentInChildren<Dogcollider>().TakeDamage(20);
                }
                else if (attackTarget.GetComponentInChildren<EnemyTu>() != null)
                {
                    attackTarget.GetComponentInChildren<EnemyTu>().TakeDamage(20);
                }
            }
            else
            { // Player 
                attackTarget?.GetComponentInParent<TankController2>()?.TakeDamage(10);
                attackTarget?.GetComponentInParent<TankController3D>()?.TakeDamage(20);
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

        public override void TakeDamage(float damage)
        {
            Debug.Log(damage);
            damage = (float)(damage * takeDamageRatio);
            currentHealth -= damage;
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
            if (currentHealth <= 0)
            {
                Death();
            }
        }

        public override void Death()
        {
            currentHealth = 0;
            currentHealthBar.transform.localScale = new Vector3(0, currentHealthBar.transform.localScale.y);
            EnemyFactory.enemies.Remove(self);
            Invoke("DestroySelf", 1);
        }

        private void DestroySelf()
        {
            EnemyBoss_Level2.listChild.Remove(self);
            Destroy(self);
        }

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
            return EnemyType.BOSS_LEVEL_2_CHILD;
        }

        public override void UpgrageLevelCorrespondToPhase(Phase phase)
        {
            throw new System.NotImplementedException();
        }

        //public override void Instantiate()
        //{
            // throw new System.NotImplementedException();
        //}

        public override void ReceiveHealthBumpFromBoss()
        {
            Invoke("HandleReceiveHealthBumpFromBoss", 1.5f);
        }

        private void HandleReceiveHealthBumpFromBoss()
        {
            if (currentHealth <= 30 && currentHealth > 0)
            {
                currentHealth += 30;
                currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
                effectBuff.SetActive(true);
                effectBuff.GetComponentInChildren<ParticleSystem>().Play();
            }
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

