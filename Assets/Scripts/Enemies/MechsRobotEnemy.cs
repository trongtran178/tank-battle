using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Enemies;

namespace Assets.Scripts.Enemies
{
    public class MechsRobotEnemy : Enemy
    {
        public float minimumDistanceIndicatorBetweenAttackTarget = 24;

        public GameObject self;
        public GameObject currentHealthBar;
        public GameObject weaponLeft;
        public GameObject weaponRight;
        public GameObject projectile;
        public GameObject effectTakeDamage;
        public GameObject effectBuff;
        public GameObject effectDamaged;
        public GameObject effectDestroy;
        public GameObject firePoint;

        private Animator animator; // include jump, idle, attack, fall back.
        private Animator weaponLeftAnimator;
        private Animator weaponRightAnimator;

        private float currentHealth;
        private float takeDamageRatio = 1;
        /// <summary>
        ///  If horizontalMove negative, enemy will moving in left side,
        ///  else if horizontalMove is positive, enemy will moving in right side,
        ///  else enemy will Idle
        /// </summary>
        private float horizontalMove = 0;

        private Rigidbody2D rigidBody2D;

        // Su dung de xu ly di chuyen
        private Vector3 Velocity = Vector3.zero;

        void Awake()
        {
            animator = GetComponentInParent<Animator>();
            currentHealth = maxHealth;
            effectBuff.SetActive(false);
            minimumDistanceIndicatorBetweenAttackTarget = GetRandomMinimumDistance(minimumDistanceIndicatorBetweenAttackTarget);
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
            weaponLeftAnimator = weaponLeft.GetComponent<Animator>();
            weaponRightAnimator = weaponRight.GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            attackTarget = FindAttackTarget();
            animator.Play("Walk");
            weaponLeftAnimator.Play("Idle");
            weaponRightAnimator.Play("Idle");
            InvokeRepeating("HandleAttack", .0f, 3.0f);
        }

        // Update is called once per frame
        void Update()
        {
            if(currentHealth <= 0)
            {
                Death();
                return;
            }
            attackTarget = FindAttackTarget();
            if (currentHealth > 0) Move();
            if (!attackTarget) CancelInvoke("HandleAttack");
        }

        private void Move()
        {
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                animator.Play("Jump_simple");
                weaponLeftAnimator.Play("Idle");
                weaponRightAnimator.Play("Idle");
                return;
            }
            //  Neu enemy het mau thi khong the di chuyen duoc
            else if (currentHealth <= 0) return;
            else if (attackTarget.activeSelf)
            {
                if (IsFlip())
                {
                    self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, 90, self.transform.eulerAngles.z);
                }
                else
                {
                    self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, -90, self.transform.eulerAngles.z);
                }

                float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
                if (distanceBetweenAttackTarget >= (minimumDistanceIndicatorBetweenAttackTarget - 8)
                    && distanceBetweenAttackTarget <= (minimumDistanceIndicatorBetweenAttackTarget + 8))
                {
                    // Attack!!! 
                    horizontalMove = 0;
                    animator.Play("Idle");
                    weaponLeftAnimator.Play("Shoot");
                    weaponRightAnimator.Play("Shoot");
                }
                else if (distanceBetweenAttackTarget > minimumDistanceIndicatorBetweenAttackTarget + 8)
                {
                    horizontalMove = moveSpeed * (IsFlip() ? 1 : -1);
                    animator.Play("Walk");

                    weaponLeftAnimator.Play("Idle");
                    weaponRightAnimator.Play("Idle");
                }
                else if (distanceBetweenAttackTarget < minimumDistanceIndicatorBetweenAttackTarget - 8)
                {
                    //horizontalMove = moveSpeed;
                    horizontalMove = 0;
                    animator.Play("Idle");
                    weaponLeftAnimator.Play("Shoot");
                    weaponRightAnimator.Play("Shoot");
                }
            }
        }

        private void HandleAttack()
        {
            if (currentHealth <= 0 || !self.activeSelf || self == null)
            {
                CancelInvoke("HandleAttack");
                return;
            }
            if (attackTarget != null) { 
                float distanceBetweenAttackTarget = Vector2.Distance(attackTarget.transform.position, transform.position);
                if (distanceBetweenAttackTarget < minimumDistanceIndicatorBetweenAttackTarget + 8)
                {
                    GameObject _projectile = projectile;
                    projectile.GetComponentInChildren<MechsRobotProjectileMove>().attackTarget = attackTarget;
                    projectile.GetComponentInChildren<MechsRobotProjectileMove>().isFlip = IsFlip();
                    //weaponLeft.transform.SetPositionAndRotation(weaponLeft.transform.position, new Quaternion())
                    _projectile.SetActive(true);
                    Instantiate(_projectile, firePoint.transform.position, firePoint.transform.rotation);
                }
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

        private bool IsFlip()
        {
            if (attackTarget.transform.position.x - transform.position.x > 0)
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
                TakeDamage(bullet.damage);
            }
        }


        public override void TakeDamage(float damage)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(FreezeRotation(.1f));
            damage = (float)(damage * takeDamageRatio);
            currentHealth -= damage;

            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
            if (currentHealth <= 0)
            {
                Death();
            }
            // Almost out of blood
            else if (currentHealth <= maxHealth / 2)
            {
                effectDamaged.SetActive(true);
            }
        }

        public override void Death()
        {
            currentHealth = 0;
            animator.Play("Fall_back");
            weaponLeftAnimator.Play("Idle");
            weaponRightAnimator.Play("Idle");
            currentHealthBar.transform.localScale = new Vector3(0, currentHealthBar.transform.localScale.y);
            Invoke("DestroySelf", 2);
        }

        private void DestroySelf()
        {
            EnemyFactory.enemies.Remove(self);
            Destroy(self);
        }

        private IEnumerator FreezeRotation(float time)
        {
            yield return new WaitForSeconds(time);
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private double getAttackCorner()
        {
            if (!attackTarget) return -1;
            float x = Mathf.Abs(Mathf.Abs(self.transform.position.x) - Mathf.Abs(attackTarget.transform.position.x));
            float y = Mathf.Abs(Mathf.Abs(self.transform.position.y) - Mathf.Abs(attackTarget.transform.position.y));
            return (Mathf.Atan(y / x) * (180 / 3.14));
        }

        public float GetRandomMinimumDistance(float distance)
        {
            System.Random random = new System.Random();
            return (float)random.NextDouble() * ((distance + 2.0f) - (distance - 2.0f)) + (distance - 2.0f);
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
                currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
                effectBuff.SetActive(true);
                effectBuff.GetComponentInChildren<ParticleSystem>().Play();
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

        public override EnemyType GetEnemyType()
        {
            return EnemyType.MECHS_ROBOT;
        }

        public override void UpgrageLevelCorrespondToPhase(Phase phase)
        {
            throw new System.NotImplementedException();
        }

        public override void Instantiate()
        {
            throw new System.NotImplementedException();
        }

        public override GameObject GetSelf()
        {
            return self;
        }


        public override bool IsShortRangeStrike()
        {
            return false;
        }

    }
}