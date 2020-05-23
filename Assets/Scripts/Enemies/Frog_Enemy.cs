using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class FrogEnemy : Enemy
    {

        public float attackSpeed;
        public float maxHealth = 100.0f;
        public float moveSpeed = 90.0f;

        private float currentHealth;
        private GameObject player;
        private GameObject player_body;
        private Rigidbody2D rigidBody;

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
                this.GetComponent<Animation>().Play("Mon_T_Jump");
                return;
            }

            if (currentHealth <= 0) return;


            float distanceBetweenPlayer = Vector2.Distance(player_body.transform.position, transform.position);
            if (distanceBetweenPlayer >= 7)
            {
                CancelInvoke("Attack");
                this.GetComponent<Animation>().Play("Mon_T_Run");
                if (isFlip())
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 90, this.transform.eulerAngles.z);
                    horizontalMove = 1 * moveSpeed;
                }
                else
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -90, this.transform.eulerAngles.z);
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
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f, rigidBody.velocity.y);
            rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref Velocity, .05f);
        }


        private void HandleAttack()
        {
            if (player == null) return;
            if (currentHealth <= 0) return;
            float distanceBetweenPlayer = Vector2.Distance(player_body.transform.position, transform.position);

            if (distanceBetweenPlayer >= 7)
            {

            }
        }


        private bool isFlip()
        {
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
                TakeDamage();
            }
        }

        private void TakeDamage()
        {

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
