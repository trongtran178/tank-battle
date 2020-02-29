using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void GoToFightWithTankMode()
    { 
        SceneManager.LoadScene("SampleScene");
    }
    public void GoToMap2()
    {
        SceneManager.LoadScene("Map2");
    }
    public void Exit()
    {
        Application.Quit();

        Debug.Log("exit");
    }

}
