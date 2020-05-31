using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class FrogEnemy : Enemy
    {

        public float speedAttack;
        public float maxHealth = 100.0f;
        public float moveSpeed = 50.0f;

        private float currentHealth;
        private GameObject player;
        private GameObject player_body;
        private Rigidbody2D rigidBody;
        public GameObject self;
        private GameObject healthBar;
        /// <summary>
        ///  If horizontalMove negative, enemy will moving in left side,
        ///  else if horizontalMove is positive, enemy will moving in right side,
        ///  else enemy will Idle
        /// </summary>
        private float horizontalMove = 0;

        private Vector3 Velocity = Vector3.zero;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("player");
            player_body = GameObject.FindGameObjectWithTag("player_body");
            //healthBar = GetComponentInParent()/
            rigidBody = GetComponentInParent<Rigidbody2D>();
            
            currentHealth = maxHealth;
        }

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("HandleAttack", .1f, .1f);
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (player == null)
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
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, rigidBody.velocity.y);
            rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref Velocity, .05f);
        }

        private void HandleAttack()
        {
            if (player == null) return;
            if (currentHealth <= 0) return;
            float distanceBetweenPlayer = Vector2.Distance(player_body.transform.position, transform.position);

            if (self.GetComponent<Animation>().IsPlaying("Mon_T_Attack")) return;
            else if (distanceBetweenPlayer < 7)
            {
                player.GetComponent<TankController2>().TakeDamage(20);
                self.GetComponent<Animation>().Play("Mon_T_Attack");
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
           
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            Bullet bullet = collision.GetComponent<Bullet>();

            if (bullet != null)
            {
                //currentHealth -= bullet.damage;
                //healthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, healthBar.transform.localScale.y);
                //if (currentHealth <= 0)
                //{
                //    self.GetComponent<Animation>().Play("Mon_T_Dead");
                //    // Invoke("Death", 2);
                //}
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
