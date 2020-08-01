using System.Collections;
using UnityEngine;
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
        private float takeDamageRatio = .02f;
        private bool isDeath = false;
        public static ArrayList listChild;
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
            listChild = new ArrayList();
        }

        void Start()
        {
            IgnoreEnemies();
            //attackTarget = FindAttackTarget();
            animator.Play("Idle");
            InvokeRepeating("HandleAttack", .1f, .1f);
            InvokeRepeating("HandleGenerateChild", 25.0f, 25.0f);
            moveSpeed = 80.0f;
        }

        void Update()
        {

            if (currentHealth <= 0)
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
            if (attackTarget == null)
            {
                animator.Play("Idle");
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }

            }
            else
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                    {
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        // GameObject.FindGameObjectWithTag("player").GetComponent<TankController3D>().
                    }

                }
            }
            if (attackTarget == null) return;
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
            if (attackTarget == null)
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                    {
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    }

                }

            }
            else
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                    {
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    }

                }
            }
            if (attackTarget == null) return;
            if (currentHealth <= 0) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Skill")) return;
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .05f);
        }

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
        }

        private void HandleAttack()
        {
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }
                return;
            }
            else
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                    {
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    }

                }
            }

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
            if (attackTarget == null || attackTarget.activeSelf == false)
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
            else
            {
                foreach (GameObject child in listChild)
                {
                    if (child != null && child.activeSelf == true)
                    {
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        child.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    }

                }
            }

            if (attackTarget == null || attackTarget.activeSelf == false)
                return;

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
                attackTarget.GetComponentInParent<TankController2>()?.TakeDamage(30);
                attackTarget.GetComponentInParent<TankController3D>()?.TakeDamage(30);
            }

        }

        private void HandleMinimumDistanceIndicatorBetweenAttackTarget()
        {

            if (attackTarget.GetComponentInChildren<PlaneControiler>() != null)
            {
                minimumDistanceIndicatorBetweenAttackTarget = 20;
            }
            else
            {
                minimumDistanceIndicatorBetweenAttackTarget = 15;
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

            EnemyFactory.enemies.Remove(self);
            foreach (GameObject child in listChild)
            {
                if (child != null && child.activeSelf == true)
                {
                    Destroy(child);
                }

            }
            Invoke("DestroySelf", 2.0f);
        }

        private void DestroySelf()
        {
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
            if (listChild.Count > 15)
                return;
            // else isGenerating = false;


            if (attackTarget == null) return;
            if (isDeath || currentHealth <= 0) return;

            animator.Play("Skill");
            StartCoroutine(HandleGenerateChildWaiter());
        }

        IEnumerator HandleGenerateChildWaiter()
        {
            GameObject childObject1 = Instantiate(child, new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z), child.transform.rotation);
            childObject1.SetActive(true);
            childObject1.transform.localScale = new Vector3(4.0f, 4.0f, 6.0f);
            listChild.Add(childObject1);
            yield return new WaitForSeconds(.2f);

            GameObject childObject2 = Instantiate(child, new Vector3(self.transform.position.x - 8, self.transform.position.y, self.transform.position.z), child.transform.rotation);
            childObject2.SetActive(true);
            childObject2.transform.localScale = new Vector3(4.0f, 4.0f, 6.0f);
            listChild.Add(childObject2);
            yield return new WaitForSeconds(.4f);

            GameObject childObject3 = Instantiate(child, new Vector3(self.transform.position.x + 8, self.transform.position.y, self.transform.position.z), child.transform.rotation);
            childObject3.SetActive(true);
            childObject3.transform.localScale = new Vector3(4.0f, 4.0f, 6.0f);
            listChild.Add(childObject3);
            yield return new WaitForSeconds(.5f);

        }

        public override void UpgrageLevelCorrespondToPhase(Phase phase)
        {
            throw new System.NotImplementedException();
        }

        //public override void Instantiate()
        //{
        //    throw new System.NotImplementedException();
        //}

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
