using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class MenuControiler : MonoBehaviour
{

    public string menu;

    public GameObject pauseMenu;

    // ENEMY AREA

    public GameObject enemyHouse;

    public GameObject mechsRobot;

    public GameObject frog;

    public GameObject boss;

    // END OF ENEMY AREA

    // ALLIES AREA

    public GameObject alliesDog;

    public GameObject alliesTank;

    public GameObject alliesPlane;

    // END OF ALLIES AREA

    public GameObject player;

    public GameObject manageRecoveryTime;

    public bool isPaused;

    private GameObject gun;

   
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.Find("Gun");
    }

    // Update is called once per frame
    void Update()
    {

        //tank = GameObject.Find("Tank2");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {

                ResumeGame();
            }
            else
            {
                isPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                gun.SetActive(false);
            }

        }

        //if(tank==null)
        //{
        //    pauseMenu.SetActive(true);

        //}
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gun.SetActive(true);
    }

    public void ReturnGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menu);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGameFactory(player, enemyHouse.GetComponentInChildren<EnemyFactory>(), manageRecoveryTime);
    }

    public void LoadGame()
    {
        // Destroy all enemies
        foreach (GameObject enemy in EnemyFactory.enemies)
        {
            Destroy(enemy);
        }

        // Destroy all allies
        foreach (GameObject allies in GameObject.FindGameObjectsWithTag("allies"))
        {
            Destroy(allies);
        }

        // Destroy player
        GameObject currentPlayerObject = GameObject.FindGameObjectWithTag("player");
        // Destroy(currentPlayerObject);

        // GET PREVIOUS USER DATA
        PlayerData playerData = SaveSystem.LoadPlayer();

        currentPlayerObject.transform.position = new Vector3(playerData.PositionX, playerData.PositionY, playerData.PositionZ);
        currentPlayerObject.GetComponentInChildren<TankController2>().health = (int) playerData.CurrentHealth;

        HealthBarTank.healthTank = playerData.CurrentHealth;
        ManaTank.manaTank = playerData.CurrentMana;
        // GameObject previousPlayer = Instantiate(player, new Vector3(playerData.PositionX, playerData.PositionY, playerData.PositionZ), player.transform.rotation);



        EnemyFactory.enemies.Clear();

        EnemyFactoryData enemyFactoryData = SaveSystem.LoadEnemyFactory();

        enemyHouse.SetActive(enemyFactoryData.IsActive);

        EnemyFactory enemyFactoryScript = enemyHouse.GetComponentInChildren<EnemyFactory>();
        enemyFactoryScript.SetCurrentHealth(enemyFactoryData.CurrentHealth);
        enemyFactoryScript.SetGenerateEnemyTime(enemyFactoryData.GenerateEnemyTime);
        enemyFactoryScript.SetBurn(!enemyFactoryData.IsBurn);
        enemyFactoryScript.HandleCurrentHealthBar();

        //RE-INITIALIZE ENEMIES
        foreach (EnemyData enemyData in enemyFactoryData.EnemiesData)
        {
            switch (enemyData.Type)
            {
                case Assets.Scripts.Enemy.EnemyType.BOSS:
                    {
                        GameObject bossInit = Instantiate(boss, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), boss.transform.rotation);
                        bossInit.SetActive(true);
                        bossInit.transform.localScale = new Vector3((float)2.5, (float)4, (float)2.5);
                        bossInit.GetComponentInChildren<EnemyBoss>().SetCurrentHealth(enemyData.CurrentHeath);
                        bossInit.GetComponentInChildren<EnemyBoss>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(bossInit);
                        break;
                    }
                case Assets.Scripts.Enemy.EnemyType.FROG:
                    {
                        GameObject frogInit = Instantiate(frog, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), frog.transform.rotation);
                        frogInit.SetActive(true);
                        frogInit.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
                        frogInit.GetComponentInChildren<FrogEnemy>().SetCurrentHealth(enemyData.CurrentHeath);
                        frogInit.GetComponentInChildren<FrogEnemy>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(frogInit);
                        break;
                    }
                case Assets.Scripts.Enemy.EnemyType.MECHS_ROBOT:
                    {
                        GameObject mechsRobotInit = Instantiate(mechsRobot, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), mechsRobot.transform.rotation);
                        mechsRobotInit.SetActive(true);
                        mechsRobotInit.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
                        mechsRobotInit.GetComponentInChildren<MechsRobotEnemy>().SetCurrentHealth(enemyData.CurrentHeath);
                        mechsRobotInit.GetComponentInChildren<MechsRobotEnemy>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(mechsRobotInit);
                        break;
                    }
            }
        }


        //RE-INITIALIZE ALLIES
        ArrayList alliesData = SaveSystem.LoadAllies();
        foreach (AlliesObjectData alliesObjectData in alliesData)
        {
            switch (alliesObjectData.Type)
            {
                case AlliesType.DOG:
                    {
                        GameObject alliesDogInit = Instantiate(alliesDog, new Vector3(alliesObjectData.PositionX, alliesObjectData.PositionY, alliesObjectData.PositionZ), alliesDog.transform.rotation);
                        alliesDogInit.SetActive(true);
                        alliesDogInit.GetComponentInChildren<Dogcollider>().health = alliesObjectData.CurrentHeath;
                        break;
                    }
                case AlliesType.PLANE:
                    {
                        GameObject alliesPlaneInit = Instantiate(alliesPlane, new Vector3(alliesObjectData.PositionX, alliesObjectData.PositionY, alliesObjectData.PositionZ), alliesPlane.transform.rotation);
                        alliesPlaneInit.SetActive(true);
                        alliesPlaneInit.GetComponentInChildren<PlaneCollider>().PlaneHealth = alliesObjectData.CurrentHeath;
                        break;
                    }
                case AlliesType.TANK:
                    {
                        GameObject alliesTankInit = Instantiate(alliesTank, new Vector3(alliesObjectData.PositionX, alliesObjectData.PositionY, alliesObjectData.PositionZ), alliesTank.transform.rotation);
                        alliesTankInit.SetActive(true);
                        alliesTankInit.GetComponentInChildren<EnemyTu>().health = alliesObjectData.CurrentHeath;
                        break;
                    }
            }
        }


        // RE-INITIALIZE RECOVERY ALLIES TIME
        ArrayList initAlliesTimes = SaveSystem.LoadInitAlliesTimes();
        foreach (InitAlliesTimeData initAlliesTimeData in initAlliesTimes)
        {
            switch (initAlliesTimeData.Type)
            {
                case AlliesType.DOG:
                    {
                        ManaArmy dogManaArmy = manageRecoveryTime
                            .GetComponentsInChildren<ManaArmy>()
                            .Where(manaArmy => manaArmy.alliesObject.GetComponentInChildren<Dogcollider>() != null).ToArray()[0];

                        dogManaArmy.manaArmy = initAlliesTimeData.ManaArmy;

                        break;
                    }
                case AlliesType.PLANE:
                    {
                        ManaArmy planeManaArmy = manageRecoveryTime
                            .GetComponentsInChildren<ManaArmy>()
                            .Where(manaArmy => manaArmy.alliesObject.GetComponentInChildren<PlaneCollider>() != null).ToArray()[0];

                        planeManaArmy.manaArmy = initAlliesTimeData.ManaArmy;

                        break;
                    }
                case AlliesType.TANK:
                    {
                        ManaArmy tankManaArmy = manageRecoveryTime
                            .GetComponentsInChildren<ManaArmy>()
                            .Where(manaArmy => manaArmy.alliesObject.GetComponentInChildren<EnemyTu>() != null).ToArray()[0];

                        tankManaArmy.manaArmy = initAlliesTimeData.ManaArmy;
                        break;
                    }
            }

        }

        // RE-INITIALIZE RECOVERY BULLET TIME
        ArrayList initBulletTimes = SaveSystem.LoadInitBulletTimes();
        ManaBullet[] manaBullets = manageRecoveryTime.GetComponentsInChildren<ManaBullet>();
        for (int bulletOrder = 0; bulletOrder < initBulletTimes.Count; bulletOrder++)
        {
            switch (bulletOrder)
            {
                case 0:
                    {
                        manaBullets[bulletOrder].manaBullet = (initBulletTimes[bulletOrder] as InitBulletTimeData).BulletMana;
                        break;
                    }
                case 1:
                    {
                        manaBullets[bulletOrder].manaBullet = (initBulletTimes[bulletOrder] as InitBulletTimeData).BulletMana;
                        break;
                    }
                case 2:
                    {
                        manaBullets[bulletOrder].manaBullet = (initBulletTimes[bulletOrder] as InitBulletTimeData).BulletMana;
                        break;
                    }
            }
        }
    }
}
