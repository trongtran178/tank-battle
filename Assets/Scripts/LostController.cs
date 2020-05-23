using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostController : MonoBehaviour
{
    public string menu;

    public GameObject lostScreen;

    public GameObject WinScreen;

    public bool isLost;

    private GameObject gun;

    private GameObject tank;
    private GameObject ememy;
    private GameObject ememy1;
    private GameObject ememy2;
    private GameObject ememy3;
    private GameObject ememy4;
    private GameObject plane1;
    private GameObject soldier;
    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.Find("Gun");

    }

    // Update is called once per frame
    void Update()
    {

        tank = GameObject.Find("Tank2");
        ememy = GameObject.Find("enemy");
        ememy1 = GameObject.Find("enemy1");
        ememy2 = GameObject.Find("enemy2");
        ememy3 = GameObject.Find("enemy3");
        ememy4 = GameObject.Find("enemy4");
        plane1 = GameObject.Find("plane1");
        soldier = GameObject.Find("Soldier-4");

        //if (isLost)
        //{

        //    ResumeGame();
        //}
        //else
        //{
        //    isLost = true;
        //    lostScreen.SetActive(true);
        //    Time.timeScale = 0f;
        //    gun.SetActive(false);
        //}



        //if (tank == null)
        //{
        //    lostScreen.SetActive(true);


        //}
        //if (ememy == null && ememy1 == null && ememy2 == null && ememy3 == null && ememy4 == null && plane1 == null && soldier == null)
        //{
        //    WinScreen.SetActive(true);
        //}
    }

    public void ResumeGame()
    {
        isLost = false;
        lostScreen.SetActive(false);
        Time.timeScale = 1f;
        gun.SetActive(true);
    }
    public void ReturnGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menu);
    }
}
