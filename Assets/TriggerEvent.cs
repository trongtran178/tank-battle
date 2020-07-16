using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TriggerEvent : MonoBehaviour
{
    public Canvas detectSaveGameState;


    public GameObject menuGame;
    public GameObject enemyHouse;
    private bool isWin = false;
    private bool isLose = false;
    // Start is called before the first frame update
    void Start()
    {
        //if (Globals.IsNewGame == false)
        //{
        //    Invoke("LoadPreviousGameState", 1.0f);
        //}
    }

    void Update()
    {
        HandleWinLose();
        Debug.Log("Is win: " + isWin);
        if (isWin)
        {

        }
        else
        {

            // LOSE
        }
    }

    private void HandleWinLose()
    {
        if (enemyHouse.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetPlayer() == null)
        {
            isWin = false;
        }
        else if (enemyHouse.activeSelf == false)
        {
            switch (Globals.CurrentLevel)
            {
                case "Level1":
                    {
                        GameObject bossLevel1 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_1);
                        if (bossLevel1 == null) isWin = true;
                        break;
                    }
                case "Level2":
                    {
                        GameObject bossLevel2 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_2);
                        if (bossLevel2 == null) isWin = true;
                        break;
                    }
                case "Level3":
                    {
                        GameObject bossLevel3 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_3);
                        if (bossLevel3 == null) isWin = true;
                        break;
                    }
            }
        }
    }

    void LoadPreviousGameState()
    {
        menuGame.GetComponentInChildren<MenuController>().LoadGame();
    }
}
