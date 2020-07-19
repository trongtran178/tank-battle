using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TriggerEvent : MonoBehaviour
{
    public Canvas detectSaveGameState;


    public GameObject menuGame;
    public GameObject enemyHouse;
    public GameObject winToast;
    public GameObject loseToast;

    public bool IsWin { get; set; } = false;
    public bool IsLose { get; set; } = false;


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
        if (IsWin && !winToast.activeSelf && IsLose == false)
        {
            winToast.SetActive(true);
            menuGame.GetComponent<MenuController>().isWin = true;
        }
        if (IsLose && !loseToast.activeSelf && IsWin == false)
        {
            loseToast.SetActive(true);
            menuGame.GetComponent<MenuController>().isLose = true;
        }
    }

    private void HandleWinLose()
    {
        if (GameObject.FindGameObjectsWithTag("player").Length <= 0)
        {
            IsLose = true;
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
                            IsWin = true;
                        break;
                    }
                case "Level2":
                    {
                        GameObject bossLevel2 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_2);
                        if (bossLevel2 == null)
                            IsWin = true;
                        break;
                    }
                case "Level3":
                    {
                        GameObject bossLevel3 = GameObject
                            .FindGameObjectsWithTag("enemy")
                            .FirstOrDefault(enemy => enemy.GetComponentInChildren<Assets.Scripts.Enemies.Enemy>().GetEnemyType() == Assets.Scripts.Enemies.EnemyType.BOSS_LEVEL_3);
                        if (bossLevel3 == null)
                            IsWin = true;
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
