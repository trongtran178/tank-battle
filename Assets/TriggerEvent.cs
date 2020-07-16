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
    public GameObject winToast;
    public GameObject loseToast;
    // Start is called before the first frame update
    void Start()
    {
        winToast.SetActive(false);
        loseToast.SetActive(false);
        if (Globals.IsNewGame == false)
        {
            Invoke("LoadPreviousGameState", .2f);
        }
    }

    void Update()
    {
        HandleWinLose();
        if (isWin && !winToast.activeSelf)
        {
            winToast.SetActive(true);
            menuGame.GetComponent<MenuController>().isWin = true;
        }
        //else
        //{
        //    winToast.SetActive(false);
        //    menuGame.GetComponent<MenuController>().isWin = false;

        //}

        if (isLose && !loseToast.activeSelf)
        {
            loseToast.SetActive(true);
            menuGame.GetComponent<MenuController>().isLose = true;
        }
        //else
        //{
        //    loseToast.SetActive(false);
        //    menuGame.GetComponent<MenuController>().isLose = false;
        //}
    }

    private void HandleWinLose()
    {
        if (GameObject.FindGameObjectWithTag("player") == null)
        {
            isLose = true;
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
                        if (bossLevel1 == null)
                            isWin = true;
                        break;
                    }
                case "Level2":
                    {
                        GameObject bossLevel2 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_2);
                        if (bossLevel2 == null)
                            isWin = true;
                        break;
                    }
                case "Level3":
                    {
                        GameObject bossLevel3 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_3);
                        if (bossLevel3 == null)
                            isWin = true;
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
