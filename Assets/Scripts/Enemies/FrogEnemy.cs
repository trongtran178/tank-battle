using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class FrogEnemy : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;

        private  float currentHealth;
        private Rigidbody2D rigidBody2D;

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
            currentHealth = health;
            //player = GameObject.FindGameObjectWithTag("player");
            //player_body = GameObject.FindGameObjectWithTag("player_body");
            rigidBody2D = GetComponentInParent<Rigidbody2D>();

            attackTarget = FindAttackTarget();

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
            //if (player == null || player.activeSelf == false)
            if(attackTarget == null)
            {
                self.GetComponent<Animation>().Play("Mon_T_Jump");
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
                self.GetComponent<Animation>().Play("Mon_T_Run");
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

            //if (player == null) return;
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

            if (self.GetComponent<Animation>().IsPlaying("Mon_T_Attack")) return;
            else if (distanceBetweenAttackTarget < 7)
            {
                self.GetComponent<Animation>().Play("Mon_T_Attack");
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
                    self.GetComponent<Animation>().Play("Mon_T_Dead");
                    Invoke("Death", 2);
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

        //private GameObject FindAttackTarget()
        //{
            
        //    GameObject attackTarget = null;
        //    List<GameObject> allies = new List<GameObject>();
        //    GameObject playerTarget;
        //    // Key - value equivalent gameObject with distance between enemy
        //    Dictionary<GameObject, float> alliesDictionary = new Dictionary<GameObject, float>();

        //    playerTarget = GameObject.FindGameObjectWithTag("player_body");
        //    //playerTarget = GameObject.FindGameObjectWithTag("player");
        //    if (player == null || player.activeSelf == false) return null;
        //    allies.Add(playerTarget);

        //    // get all allies
        //    GameObject[] alliesArray = GameObject.FindGameObjectsWithTag("allies");
        //    for(int i = 0; i  < alliesArray.Length; i++)
        //    {
        //        allies.Add(alliesArray[i]);
        //    }
        //    float shortestAttackTargetDistance;
            
        //    shortestAttackTargetDistance  = Vector2.Distance(playerTarget.transform.position, transform.position);
           
        //    foreach (GameObject alliesGameObject in allies)
        //    {
        //        float distance = Vector2.Distance(alliesGameObject.transform.position, transform.position);
        //        if(shortestAttackTargetDistance >= distance)
        //        {
        //            shortestAttackTargetDistance = distance;
        //        }
        //        alliesDictionary.Add(alliesGameObject, distance);
        //    };

        //    attackTarget = alliesDictionary.FirstOrDefault(x => x.Value <= shortestAttackTargetDistance).Key;
        //    return attackTarget;
        //}
    }
}
