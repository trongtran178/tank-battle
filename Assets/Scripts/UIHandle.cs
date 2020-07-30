using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIHandle : MonoBehaviourPunCallbacks
{

    public InputField createRoomTF;
    public InputField joinRoomTF;
    public GameObject connectScreen;
    public GameObject GamePlay;
    public GameObject camera;

    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomTF.text, new RoomOptions { MaxPlayers = 4 },null);
    }
    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomTF.text, null);
    }

    public override void OnJoinedRoom()
    {
        print("Room Joined Success");
        connectScreen.SetActive(false);
        GamePlay.SetActive(true);
        camera.SetActive(false);

    }
 
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Room Join fail "+returnCode+" Message "+message);
    }
}
