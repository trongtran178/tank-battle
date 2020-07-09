using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControiler : MonoBehaviour
{

    public string menu;

    public GameObject pauseMenu;

    public GameObject enemyHouse;

    public GameObject mechsRobot;

    public GameObject frog; 

    public GameObject boss;

    public bool isPaused;
    
    private GameObject gun;

    private GameObject tank;

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
            if(isPaused)
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
        Debug.Log(enemyHouse.GetComponentInChildren<EnemyFactory>());
        SaveSystem.SaveEnemiesFactory(enemyHouse.GetComponentInChildren<EnemyFactory>());

    }

    public void LoadGame()
    {
        // Clear all enemies
        foreach(GameObject enemy in EnemyFactory.enemies)
        {       
            Destroy(enemy);
        }
        EnemyFactory.enemies.Clear();

        EnemyFactoryData enemyFactoryData = SaveSystem.LoadEnemyFactory();

        enemyHouse.SetActive(enemyFactoryData.IsActive);
        enemyHouse.GetComponentInChildren<EnemyFactory>().SetCurrentHealth(enemyFactoryData.CurrentHealth);
        enemyHouse.GetComponentInChildren<EnemyFactory>().SetBurn(!enemyFactoryData.IsBurn);
        enemyHouse.GetComponentInChildren<EnemyFactory>().HandleCurrentHealthBar();

        foreach (EnemyData enemyData in enemyFactoryData.EnemiesData)
        {
            switch(enemyData.Type)
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

    }
}
