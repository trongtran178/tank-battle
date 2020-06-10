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

        private float currentHealth;
        private float generateEnemyTime = 10.0f;
        private static ArrayList enemies;
        private System.Random random = new System.Random();
        //private GameObject player;
        private bool flag = false;

        void Awake()
        {
            currentHealth = 100;
            enemies = new ArrayList();
        }

        private void Start()
        {
            InvokeRepeating("GenerateEnemy", 1.0f, generateEnemyTime);
            //Invoke("GenerateEnemy", 1.0f);
        }

        private void Update()
        {
            if (!player) CancelInvoke("GenerateEnemy");
            if (currentHealth <= 60)
            {
                if (flag == false)
                {
                    generateEnemyTime = 6.0f;
                    CancelInvoke("GenerateEnemy");
                    InvokeRepeating("GenerateEnemy", 1.0f, generateEnemyTime);

                }
                flag = true;
            }
        }

        public void CreateEnemies(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.FROG:
                    {
                        GameObject frogInit = Instantiate(frog, new Vector3(self.transform.position.x - 20, self.transform.position.y), frog.transform.rotation);
                        frogInit.SetActive(true);
                        enemies.Add(frogInit);
                        break;
                    }
                case EnemyType.MECHS_ROBOT:
                    {
                        GameObject mechsRobotInit = Instantiate(mechsRobot, new Vector3(self.transform.position.x - 20, self.transform.position.y), mechsRobot.transform.rotation);
                        mechsRobotInit.SetActive(true);
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
                        GameObject bossInit = Instantiate(boss, new Vector3(self.transform.position.x - 20, self.transform.position.y), mechsRobot.transform.rotation);
                        bossInit.SetActive(true);
                        enemies.Add(bossInit);
                        break;
                    }
                default:
                    throw new Exception(message: "Error");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            Debug.Log(bullet);

            if (bullet != null)
            {
                // Take damage
                currentHealth -= 30;
                currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);

                if (currentHealth <= 0)
                {
                    CreateEnemies(EnemyType.BOSS);
                    Invoke("DestroyEnemyFactory", .2f);
                }
            }
        }

        private void DestroyEnemyFactory()
        { 
            effectDestroy.SetActive(true);
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
            if(currentHealth > 0)
            {
                int randomVal = random.Next(1, 10);
                CreateEnemies(randomVal % 2 == 0 ? EnemyType.MECHS_ROBOT : EnemyType.FROG);
            }
           
            
            //CreateEnemies(EnemyType.MECHS_ROBOT);
            //CreateEnemies(EnemyType.FROG);
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
