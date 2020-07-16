using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using UnityEngine;
using System.Linq;
namespace Assets.Scripts.Enemies
{
    // BOSS LEVEL 3 CAN ATTACK PLANE 
    public class EnemyBoss_Level3 : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;

        private new Animation animation; // include Attack, Idle, Run, Walk.
        private float currentHealth;
        private float minimumDistanceIndicatorBetweenAttackTarget = 10;
        private Rigidbody2D rigidBody2D;
        private float takeDamageRatio = .5f;
        private bool isDeath = false;
        private System.Random random = new System.Random();
        private int randomPushAwayAttack;
        private long countAttack = 0;
        /// <summary>
        ///  If horizontalMove negative, enemy will moving in left side,
        ///  else if horizontalMove is positive, enemy will moving in right side,
        ///  else enemy will Idle
        /// </summary>
        private float horizontalMove = 0;

        // Su dung de xu ly di chuyen
        private Vector3 Velocity = Vector3.zero;

        void Start()
        {
            animation = self.GetComponent<Animation>();
            animation["Run"].speed += 4.0f;
            rigidBody2D = self.GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
            attackTarget = FindAttackTarget();
            animation.Play("Idle");
            InvokeRepeating("HandleAttack", .1f, .1f);
            randomPushAwayAttack = random.Next(4, 7);
        }

        void Update()
        {
            if (currentHealth <= 0 || isDeath)
            {
                Death();
                return;
            }
            attackTarget = FindAttackTarget();
            if (attackTarget == null || attackTarget.activeSelf == false) return;
            HandleMinimumDistanceIndicatorBetweenAttackTarget();
            Move();

        }

        private void Move()
        {
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                CancelInvoke("HandleAttack");
                CancelInvoke("HandleMove");
                animation.Play("Idle");

                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong

            if (animation.IsPlaying("Attack")) return;

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
                animation.Play("Walk");
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

            if (countAttack > 0 && (countAttack % randomPushAwayAttack) == 0)
            {
                // HAT TUNG
                randomPushAwayAttack = random.Next(4, 7);
                countAttack = 0;
                HandlePushAway();
            }

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
                if (animation.IsPlaying("Attack")) return;
                animation.Play("Attack");
                Invoke("AttackTargetTakeDamage", 0.7f);
                self.GetComponent<Animation>()["Attack"].speed = (float)1.5;
                countAttack++;


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
            if (currentHealth <= 0) return;
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
                    attackTarget.GetComponentInChildren<EnemyTu>().TakeDamage(10);
                }
            }

            else
                player.GetComponent<TankController2>().TakeDamage(30);

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
            horizontalMove = 0;
            animation.Play("Death");

            Invoke("DestroySelf", 5);
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
            if (currentHealth <= 0 || isDeath) { 
                horizontalMove = 0;
                return;
            }
            
            damage *= takeDamageRatio;
            currentHealth -= damage;
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
            if (currentHealth <= 0)
            {
                if (isDeath == false)
                {
                    horizontalMove = 0;
                    isDeath = true;
                    Death();
                    //foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
                    //{
                    //    if (enemy.GetComponentInChildren<Enemy>() != null)
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

        private void HandleMinimumDistanceIndicatorBetweenAttackTarget()
        {

            if (attackTarget.GetComponentInChildren<PlaneControiler>() != null)
            {
                minimumDistanceIndicatorBetweenAttackTarget = 30;
            }
            else
            {
                minimumDistanceIndicatorBetweenAttackTarget = 10;
            }
        }

        // Special skill
        public void HandlePushAway()
        {
            if (currentHealth <= 0 || isDeath) return;
            ArrayList alliesObjectsPushAway = new ArrayList(5);
            List<GameObject> allAllies = GameObject.FindGameObjectsWithTag("allies").ToList();
            GameObject player = GameObject.FindGameObjectWithTag("player");
            allAllies.Add(player);
            foreach (GameObject alliesObject in allAllies)
            {

                float distanceBetweenBoss = Vector2.Distance(self.transform.position, alliesObject.transform.position);

                if (distanceBetweenBoss <= 20)
                {
                    alliesObjectsPushAway.Add(alliesObject);
                }
            }
            foreach (GameObject allies in alliesObjectsPushAway)
            {

                if (allies.transform.position.x <= self.transform.position.x + 3)
                {

                    allies.GetComponent<AlliesBeingPushAway>()?.PushAway(true);
                }
                else
                {
                    allies.GetComponent<AlliesBeingPushAway>()?.PushAway(false);
                }
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
            return EnemyType.BOSS_LEVEL_3;
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
