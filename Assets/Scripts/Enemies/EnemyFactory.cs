using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyFactory : Enemy
    {
        public GameObject currentHealthBar;
        public GameObject effectDestroy;
        public GameObject self;
        public GameObject mechsRobot;
        public GameObject frog;
        public GameObject boss;
        public GameObject takeDamagePoint;

        private float currentHealth;
        private float generateEnemyTime = 10.0f;
        private bool isBurn = false;
        private bool flag = false;
        private GameObject[] effectBurnArray;
        private System.Random random = new System.Random();
        public static ArrayList enemies;

        void Awake()
        {
            enemies = new ArrayList();
            currentHealth = maxHealth;
            effectBurnArray = GameObject.FindGameObjectsWithTag("house_fire");
            foreach (GameObject effectBurn in effectBurnArray)
            {
                effectBurn.GetComponent<ParticleSystem>().Stop();
            }
        }

        private void Start()
        {
            InvokeRepeating("GenerateEnemy", 1.0f, generateEnemyTime);
        }

        private void Update()
        {
            if (!player) CancelInvoke("GenerateEnemy");
            if (currentHealth <= 30 && !isBurn)
            {
                BurnEnemyFactory();
            }
            if (currentHealth <= 50)
            {
                if (flag == false)
                {
                    generateEnemyTime = 4.0f;
                    CancelInvoke("GenerateEnemy");
                    InvokeRepeating("GenerateEnemy", 1.0f, generateEnemyTime);
                }
                flag = true;
            }
        }

        public void CreateEnemies(EnemyType enemyType)
        {
            if (enemies.Count >= 20) return;
            switch (enemyType)
            {
                case EnemyType.FROG:
                    {
                        GameObject frogInit = Instantiate(frog, new Vector3(self.transform.position.x - 20, self.transform.position.y, self.transform.position.z), frog.transform.rotation);
                        frogInit.SetActive(true);
                        frogInit.transform.localScale = new Vector3((float)1.5, (float) 1.5, (float) 1.5);
                        enemies.Add(frogInit);
                        break;
                    }
                case EnemyType.MECHS_ROBOT:
                    {
                        GameObject mechsRobotInit = Instantiate(mechsRobot, new Vector3(self.transform.position.x - 20, self.transform.position.y + 10, self.transform.position.z), mechsRobot.transform.rotation);
                        mechsRobotInit.SetActive(true);
                        mechsRobotInit.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
                        enemies.Add(mechsRobotInit);
                        break;
                    }
                case EnemyType.SOLDIER:
                    {
                        break;
                    }
                case EnemyType.SPACECRAFT:
                    {
                        break;
                    }
                case EnemyType.BOSS:
                    {
                        GameObject bossInit = Instantiate(boss, new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z), mechsRobot.transform.rotation);
                        bossInit.SetActive(true);
                        bossInit.transform.localScale = new Vector3((float)2.5, (float)4, (float)2.5);
                        enemies.Add(bossInit);
                        break;
                    }
                default:
                    throw new Exception(message: "Error");
            }
        }

        private void BurnEnemyFactory()
        {
            if (isBurn) return;
            isBurn = true;
            Debug.Log(effectBurnArray.Length);
            foreach (GameObject effectBurn in effectBurnArray)
            {
                effectBurn.GetComponent<ParticleSystem>().Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
        }

        public override void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);

            if (currentHealth <= 0)
            {
                Invoke("DestroyEnemyFactory", .2f);
            }
        }

        private void DestroyEnemyFactory()
        {
            CreateEnemies(EnemyType.BOSS);
            effectDestroy.SetActive(true);
            effectDestroy.GetComponentInChildren<ParticleSystem>().Play();
            //GameObject ga= Instantiate(effectDestroy, transform.position, transform.rotation);
            Destroy(self);
            CancelInvoke("GenerateEnemy");

          
        }

        private void GenerateEnemy()
        {
            int count = 0;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    count++;
                }
            }
            if (currentHealth > 0)
            {
                int randomVal = random.Next(1, 10);
                //CreateEnemies(randomVal % 2 == 0 ? EnemyType.MECHS_ROBOT : EnemyType.FROG);
                // CreateEnemies( EnemyType.MECHS_ROBOT );
            }
        }

        public override void Death()
        {
            Debug.Log("ENEMY FACTORY DEATH");   
            // throw new NotImplementedException();
        }

        public override void UpgrageLevelCorrespondToPhase(Phase phase)
        {
            // DO NOTHING
            throw new System.NotImplementedException();
        }

        public override void Instantiate()
        {
            // DO NOTHING
            throw new System.NotImplementedException();
        }

        public override void ReceiveHealthBumpFromBoss()
        {
            throw new NotImplementedException();
        }

        public override void SetCurrentHealth(float currentHealth)
        {
            throw new NotImplementedException();
        }

        public override float GetCurrentHealth()
        {
            throw new NotImplementedException();
        }
    }
}
