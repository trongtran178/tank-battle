using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnection : MonoBehaviourPunCallbacks
{

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    public GameObject connectedScreen;
    public GameObject disconnectionScreen;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClick_Connection()
    {
        print("connectting to server");


        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            //PhotonNetwork.NickName = MasterManagerPhoton.GameSettingPhoton.NickName;
            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.GameVersion = MasterManagerPhoton.GameSettingPhoton.GameVersion;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    public override void OnConnectedToMaster()
    {
     
        print("connected to server");
        print(PhotonNetwork.LocalPlayer.NickName);

       
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        //PhotonNetwork.CreateRoom("MyRoom");
        //PhotonNetwork.JoinRandomRoom();

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        // print("Disconnected "+cause);
        disconnectionScreen.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        if (disconnectionScreen.activeSelf)
            disconnectionScreen.SetActive(false);
        connectedScreen.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
