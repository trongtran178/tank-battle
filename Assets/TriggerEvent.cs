﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public Canvas detectSaveGameState;

   
    public GameObject menuGame;

    // Start is called before the first frame update
    void Start()
    {
        //if (Globals.IsNewGame == false)
        //{
        //    Invoke("LoadPreviousGameState", 1.0f);
        //}
    }

    void LoadPreviousGameState()
    {
        menuGame.GetComponentInChildren<MenuController>().LoadGame();
    }
}