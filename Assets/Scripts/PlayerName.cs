using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{

    public InputField nametf;
    public Button createName;

    //public void OnTFChange()
    //{
    //    if (nametf.text.Length > 2 && nametf.text.Length < 5)
    //    {
    //        createName.interactable = true;
    //    }
    //    else createName.interactable = false;
    //}
    public void OnClick_createName()
    {
        PhotonNetwork.NickName = nametf.text;
    }
}
