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

        public static ArrayList enemies;

        private float currentHealth;
        private float generateEnemyTime = 10.0f;
        private double takeDamageRatio = .07;
        private bool isBurn = false;

        // If flag is true, generateEnemyTime will be decrease, ...
        private bool flag = false;

        private GameObject[] effectBurnArray;
        private System.Random random = new System.Random();

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
            else if (currentHealth > 30 && isBurn)
            {
                BurnEnemyFactory();
            }
            else if(currentHealth > 30 && !isBurn)
            {
                // DO NOTHING
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
            if(!isBurn)
            {
                Debug.Log(123);
                isBurn = true;
                foreach (GameObject effectBurn in effectBurnArray)
                {
                    effectBurn.GetComponent<ParticleSystem>().Play();
                }
                
            }
            else
            {
                Debug.Log(134);
                isBurn = false;
                foreach (GameObject effectBurn in effectBurnArray)
                {
                    effectBurn.GetComponent<ParticleSystem>().Stop();
                }
                
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
            damage = (int) ((float)damage * takeDamageRatio);
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
            self.SetActive(false);
            CancelInvoke("GenerateEnemy");
        }

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((currentHealth / 100) > 0 ? (currentHealth / 100) : 0, currentHealthBar.transform.localScale.y);
        }

        public void HandleBurnHouse()
        {

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
                CreateEnemies(randomVal % 2 == 0 ? EnemyType.MECHS_ROBOT : EnemyType.FROG);
            }
        }


        public ArrayList getEnemies()
        {
            return enemies;
        }

        public void setEnemies(ArrayList listEnemies)
        {
            enemies = listEnemies;
        }

        public bool IsBurn()
        {
            return isBurn;
        }

        public void SetBurn(bool isBurn)
        {
            this.isBurn = isBurn;
        }

        public bool GetFlag()
        {
            return flag;
        }

        public void SetFlag(bool flag)
        {
            this.flag = flag;
        }

        public float GetGenerateEnemyTime()
        {
            return generateEnemyTime;
        }

        public void SetGenerateEnemyTime(float generateEnemyTime) 
        {
            this.generateEnemyTime = generateEnemyTime;
        }

        ///////////////////////////////////
        ////////// OVERRIDE AREA //////////
        ///////////////////////////////////
        public override void Death()
        {
            // DO NOTHING
            Debug.Log("ENEMY FACTORY DEATH");   
            throw new NotImplementedException();
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
            // DO NOTHING
            throw new NotImplementedException();
        }

        public override EnemyType GetEnemyType()
        {
            return EnemyType.FACTORY;
        }

        public override void SetCurrentHealth(float currentHealth)
        {
            this.currentHealth = currentHealth;
        }

        public override float GetCurrentHealth()
        {
            return currentHealth;
        }

        public override GameObject GetSelf()
        {
            return self;
        }

        public override bool IsShortRangeStrike()
        {
            throw new NotImplementedException();
        }
    }
}
