using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControiler : MonoBehaviour
{

    public string menu;

    public GameObject pauseMenu;
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
}
