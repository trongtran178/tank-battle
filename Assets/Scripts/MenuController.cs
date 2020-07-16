using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.IO;

public class MenuController : MonoBehaviour
{

    public string menu;

    public GameObject pauseMenu;

    public GameObject saveGameButton;
    
    public GameObject loadGameButton;

    // ENEMY AREA

    public GameObject enemyHouse;

    public GameObject mechsRobot;

    public GameObject frog;

    public GameObject bossLevel1;

    public GameObject bossLevel2;

    public GameObject bossLevel2_Child;

    public GameObject bossLevel3;

    // END OF ENEMY AREA

    // ALLIES AREA

    public GameObject alliesDog;

    public GameObject alliesTank;

    public GameObject alliesPlane;

    // END OF ALLIES AREA

    public GameObject player;

    public GameObject manageRecoveryTime;

    public bool isPaused;

    public bool isWin = false;

    public bool isLose = false;

    private GameObject gun;

    private string playerPath;
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.Find("Gun");
        playerPath =  Application.persistentDataPath + "/player.fun";
    }

    // Update is called once per frame
    void Update()
    {
        if(!File.Exists(playerPath))
        {
            loadGameButton.SetActive(false);
        }

        else
        {
            loadGameButton.SetActive(true);
        }

        if ((isWin == true || isLose == true))
        {
            saveGameButton.SetActive(false);
            loadGameButton.SetActive(false);
        }

        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Debug.Log(Application.persistentDataPath + "/player.fun");
                ResumeGame();
            }
            else
            {
                isPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                gun?.SetActive(false);
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
        SceneManager.LoadScene("Menu");
    }

    public void SaveGame()
    {
        try
        {
            SaveSystem.SaveGameFactory(player, enemyHouse.GetComponentInChildren<EnemyFactory>(), manageRecoveryTime);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        ResumeGame();
    }

    public void LoadGame()
    {
        // Destroy all enemies

        foreach (GameObject enemy in EnemyFactory.enemies)
        {
            Destroy(enemy);
        }

        GameObject[] alliesArray = GameObject.FindGameObjectsWithTag("allies");
        // Destroy all allies
        for (int i = 0; i < alliesArray.Length; i++)
        {
            Destroy(alliesArray[i]);
        }

        // Destroy player
        GameObject currentPlayerObject = GameObject.FindGameObjectWithTag("player");
        // Destroy(currentPlayerObject);

        // GET PREVIOUS USER DATA
        PlayerData playerData = SaveSystem.LoadPlayer();
        if (playerData != null)
        {
            currentPlayerObject.transform.position = new Vector3(playerData.PositionX, playerData.PositionY, playerData.PositionZ);
            currentPlayerObject.GetComponentInChildren<TankController2>().health = (int)playerData.CurrentHealth;
        }
        HealthBarTank.healthTank = playerData.CurrentHealth;
        ManaTank.manaTank = playerData.CurrentMana;

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
                case EnemyType.FROG:
                    {
                        GameObject frogInit = Instantiate(frog, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), frog.transform.rotation);
                        frogInit.SetActive(true);
                        frogInit.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
                        frogInit.GetComponentInChildren<FrogEnemy>().SetCurrentHealth(enemyData.CurrentHeath);
                        frogInit.GetComponentInChildren<FrogEnemy>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(frogInit);
                        break;
                    }
                case EnemyType.MECHS_ROBOT:
                    {
                        GameObject mechsRobotInit = Instantiate(mechsRobot, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), mechsRobot.transform.rotation);
                        mechsRobotInit.SetActive(true);
                        mechsRobotInit.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
                        mechsRobotInit.GetComponentInChildren<MechsRobotEnemy>().SetCurrentHealth(enemyData.CurrentHeath);
                        mechsRobotInit.GetComponentInChildren<MechsRobotEnemy>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(mechsRobotInit);
                        break;
                    }
                case EnemyType.BOSS_LEVEL_1:
                    {
                        GameObject bossLevle1Init = Instantiate(bossLevel1, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), bossLevel1.transform.rotation);
                        bossLevle1Init.SetActive(true);
                        bossLevle1Init.transform.localScale = new Vector3((float)2.5, (float)4, (float)2.5);
                        bossLevle1Init.GetComponentInChildren<EnemyBoss_Level1>().SetCurrentHealth(enemyData.CurrentHeath);
                        bossLevle1Init.GetComponentInChildren<EnemyBoss_Level1>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(bossLevle1Init);
                        break;
                    }
                case Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_2:
                    {
                        GameObject bossLevel2Init = Instantiate(bossLevel2, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), bossLevel2.transform.rotation);
                        bossLevel2Init.SetActive(true);
                        bossLevel2Init.transform.localScale = new Vector3(120, 120, 120);
                        bossLevel2Init.GetComponentInChildren<EnemyBoss_Level2>().SetCurrentHealth(enemyData.CurrentHeath);
                        bossLevel2Init.GetComponentInChildren<EnemyBoss_Level2>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(bossLevel2Init);
                        break;
                    }
                case EnemyType.BOSS_LEVEL_2_CHILD:
                    {
                        GameObject bossLevel2ChildInit = Instantiate(bossLevel2_Child, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), bossLevel2_Child.transform.rotation);
                        bossLevel2ChildInit.SetActive(true);
                        bossLevel2ChildInit.transform.localScale = new Vector3(2, 2, 2);
                        bossLevel2ChildInit.GetComponentInChildren<EnemyBoss_Level2_Child>().SetCurrentHealth(enemyData.CurrentHeath);
                        bossLevel2ChildInit.GetComponentInChildren<EnemyBoss_Level2_Child>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(bossLevel2ChildInit);
                        break;
                    }
                case EnemyType.BOSS_LEVEL_3:
                    {
                        GameObject bossLevel3Init = Instantiate(bossLevel3, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), bossLevel3.transform.rotation);
                        bossLevel3Init.SetActive(true);
                        bossLevel3Init.transform.localScale = new Vector3(.7f, .7f, .7f);
                        bossLevel3Init.GetComponentInChildren<EnemyBoss_Level3>().SetCurrentHealth(enemyData.CurrentHeath);
                        bossLevel3Init.GetComponentInChildren<EnemyBoss_Level3>().HandleCurrentHealthBar();
                        EnemyFactory.enemies.Add(bossLevel3Init);
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
                        alliesDogInit.GetComponentInChildren<Dogcollider>().RepaintHealthBar();
                        //if (alliesObjectData.IsDizzy)
                        //    Invoke();
                        //    alliesDogInit.GetComponent<dog>().Dizzy();
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
                        //if (alliesObjectData.IsDizzy)
                        //    alliesTankInit.GetComponent<EnemyTu>().Dizzy();
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

        ResumeGame();
    }
}
