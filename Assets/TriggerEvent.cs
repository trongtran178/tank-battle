using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public Canvas detectSaveGameState;

    private bool isShow = false;
    public bool IsShow { get => isShow; set => isShow = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            IsShow = !IsShow;
            if(IsShow)
            {
            }
        }
    }
}
