using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Enemies
{
    public class EnemyFactory : Enemy
    {

        public GameObject currentHealthBar;
        public GameObject effectDestroy;
        public GameObject self;
        public GameObject mechsRobot;
        public GameObject frog;
        public GameObject bossLevel1;
        public GameObject bossLevel2;
        public GameObject bossLevel3;
        public GameObject takeDamagePoint;

        public static ArrayList enemies;

        private float currentHealth;
        private float generateEnemyTime = 10.0f;
        //private float takeDamageRatio = .5f;
        private float takeDamageRatio = 1.0f;
        private bool isBurn = false;

        // If flag is true, generateEnemyTime will be decrease, ...
        private bool flag = false;
        private bool isGenerating;
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
            Globals.CurrentLevel = SceneManager.GetActiveScene().name;

            if (GameObject.FindGameObjectWithTag("player") != null)
            {
                isGenerating = true;
                InvokeRepeating("GenerateEnemy", 1.0f, generateEnemyTime);
            }
        }

        private void Update()
        {
            if (currentHealth <= 30 && !isBurn)
            {
                BurnEnemyFactory();
            }
            else if (currentHealth > 30 && isBurn)
            {
                BurnEnemyFactory();
            }
            else if (currentHealth > 30 && !isBurn)
            {
                // DO NOTHING
            }

            if (GameObject.FindGameObjectWithTag("player") == null)
            {
                CancelInvoke("GenerateEnemy");
                isGenerating = false;
                return;
            } 
            else if(GameObject.FindGameObjectWithTag("player") != null && isGenerating == false)
            {
                isGenerating = true;
                InvokeRepeating("GenerateEnemy", 1.0f, generateEnemyTime);
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
            if (enemies.Count >= 15) return;
            switch (enemyType)
            {
                case EnemyType.FROG:
                    {
                        GameObject frogInit = Instantiate(frog, new Vector3(self.transform.position.x - 18, self.transform.position.y, self.transform.position.z), frog.transform.rotation);
                        frogInit.SetActive(true);
                        frogInit.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
                        enemies.Add(frogInit);
                        break;
                    }
                case EnemyType.MECHS_ROBOT:
                    {
                        GameObject mechsRobotInit = Instantiate(mechsRobot, new Vector3(self.transform.position.x - 18, self.transform.position.y, self.transform.position.z), mechsRobot.transform.rotation);
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
                case EnemyType.BOSS_LEVEL_1:
                    {
                        GameObject bossLevel1Init = Instantiate(bossLevel1, new Vector3(self.transform.position.x - 18, self.transform.position.y, self.transform.position.z), bossLevel1.transform.rotation);
                        bossLevel1Init.SetActive(true);
                        bossLevel1Init.transform.localScale = new Vector3(2.5f, 4.0f, 2.5f);
                        enemies.Add(bossLevel1Init);
                        break;
                    }
                case EnemyType.BOSS_LEVEL_2:
                    {
                        GameObject bossLevel2Init = Instantiate(bossLevel2, new Vector3(self.transform.position.x - 18, self.transform.position.y, self.transform.position.z), bossLevel2.transform.rotation);
                        bossLevel2Init.SetActive(true);
                        bossLevel2Init.transform.localScale = new Vector3(120, 120, 120);
                        enemies.Add(bossLevel2Init);
                        break;
                    }
                case EnemyType.BOSS_LEVEL_3:
                    {
                        GameObject bossLevel3Init = Instantiate(bossLevel3, new Vector3(self.transform.position.x - 18, self.transform.position.y, self.transform.position.z), bossLevel3.transform.rotation);
                        bossLevel3Init.SetActive(true);
                        bossLevel3Init.transform.localScale = new Vector3(.7f, .7f, .7f);
                        enemies.Add(bossLevel3Init);
                        break;
                    }
                default:
                    throw new Exception(message: "Error");
            }
        }

        private void BurnEnemyFactory()
        {
            if (!isBurn)
            {
                isBurn = true;
                foreach (GameObject effectBurn in effectBurnArray)
                {
                    effectBurn.GetComponent<ParticleSystem>().Play();

                }
                switch (Globals.CurrentLevel)
                {
                    case "Level1":
                        {
                            if (GameObject.FindGameObjectsWithTag("enemy")
                                         .FirstOrDefault(x => x.GetComponentInChildren<Enemy>()?.GetEnemyType() == EnemyType.BOSS_LEVEL_1) == null)
                                CreateEnemies(EnemyType.BOSS_LEVEL_1);
                            break;
                        }
                    case "Level2":
                        {
                            if (GameObject.FindGameObjectsWithTag("enemy")
                                         .FirstOrDefault(x => x.GetComponentInChildren<Enemy>()?.GetEnemyType() == EnemyType.BOSS_LEVEL_2) == null)
                                CreateEnemies(EnemyType.BOSS_LEVEL_2);
                            break;
                        }
                    case "Level3":
                        {
                            if (GameObject.FindGameObjectsWithTag("enemy")
                                         .FirstOrDefault(x => x.GetComponentInChildren<Enemy>()?.GetEnemyType() == EnemyType.BOSS_LEVEL_3) == null)

                                CreateEnemies(EnemyType.BOSS_LEVEL_3);
                            break;
                        }
                }

            }
            else
            {
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

        public override void TakeDamage(float damage)
        {
            damage = (float)(damage * takeDamageRatio);
            currentHealth -= damage;
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);

            if (currentHealth <= 0)
            {
                Invoke("DestroyEnemyFactory", .2f);
            }
        }

        private void DestroyEnemyFactory()
        {
            // CreateEnemies(EnemyType.BOSS_LEVEL_1);
            effectDestroy.SetActive(true);
            effectDestroy.GetComponentInChildren<ParticleSystem>().Play();
            self.SetActive(false);
            CancelInvoke("GenerateEnemy");
        }

        public void HandleCurrentHealthBar()
        {
            currentHealthBar.transform.localScale = new Vector3((float)((currentHealth / 100) > 0 ? (currentHealth / 100) : 0), currentHealthBar.transform.localScale.y);
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
            //// DO NOTHING
            //Debug.Log("ENEMY FACTORY DEATH");   
            //throw new NotImplementedException();
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
            // throw new NotImplementedException();
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
