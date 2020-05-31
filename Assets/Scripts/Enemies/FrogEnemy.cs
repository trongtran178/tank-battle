using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class FrogEnemy : Enemy
    {
        public GameObject self;
        public GameObject currentHealthBar;

        private  float currentHealth;
        private GameObject player;
        private GameObject player_body;
        private Rigidbody2D rigidBody2D;
    
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
            currentHealth = health;

            player = GameObject.FindGameObjectWithTag("player");
            player_body = GameObject.FindGameObjectWithTag("player_body");
            rigidBody2D = GetComponentInParent<Rigidbody2D>();
        }

        void Start()
        {
            InvokeRepeating("HandleAttack", .1f, .1f);
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (player == null || player.activeSelf == false)
            {
                self.GetComponent<Animation>().Play("Mon_T_Jump");
                return;
            }

            // Neu het mau thi khong the tan cong duoc nua
            if (currentHealth <= 0) return;
            // Neu dang tan cong thi khong chay animation va k tan cong nua, doi den khi 1 luot danh thanh cong
            if (self.GetComponent<Animation>().IsPlaying("Mon_T_Attack")) return; 

            
            float distanceBetweenPlayer = Vector2.Distance(player_body.transform.position, transform.position);
            if (distanceBetweenPlayer >= 7)
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
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref Velocity, .05f);
        }

        private void HandleAttack()
        {
            if (player == null) return;
            if (currentHealth <= 0) return;
            float distanceBetweenPlayer = Vector2.Distance(player_body.transform.position, transform.position);

            if (self.GetComponent<Animation>().IsPlaying("Mon_T_Attack")) return;
            // self.GetComponent<Animation>("aa")
            else if (distanceBetweenPlayer < 7)
            {
                player.GetComponent<TankController2>().TakeDamage(20);
                self.GetComponent<Animation>().Play("Mon_T_Attack");

                /////////////////////////////////////////
                /////////// IMPORTANT CODE  /////////////
                /////////// SET SPEED OF ANIMATION //////
                /////////////////////////////////////////
                // self.GetComponent<Animation>()["Mon_T_Attack"].speed = (float) 2.3;
            }
        }

        private bool IsFlip()
        {
            if (player_body.transform.position.x - transform.position.x > 0)
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
          
            // Bi trung dan tu player
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
    }
}
