using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private string gameVersion = "1";

    private bool isMpGame = false;

    private bool inRoom = false;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool IsMpGame()
    {
        return isMpGame;
    }

    public bool IsMpReady()
    {
        return PhotonNetwork.PlayerList.Length == 2;
    }

    public bool IsHost()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public void Connect()
    {
        Debug.Log("Network: Trying to connect...");
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("... already Connected");
            return;
        }

        isMpGame = true;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;

        Debug.Log("... connecting ...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("... Connected!");
        JoinRoom();
    }

    public bool IsInRoom()
    {
        return inRoom;
    }

    public void JoinRoom()
    {
        Debug.Log("Join Room ...");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOptions, TypedLobby.Default);
        // PhotonNetwork.CreateRoom("TestRoom");
    }

    public override void OnCreatedRoom()
    {
        inRoom = true;
        Debug.Log("... Room created!");
    }

    public override void OnJoinedRoom()
    {
        inRoom = true;
        Debug.Log("... Room joined!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("... Room creation failed!");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("... Room join failed!");
    }

    public void Disconnect()
    {
        Debug.Log("Network: Trying to disconnect...");
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("... not Connected");
        }

        PhotonNetwork.Disconnect();
        Debug.Log("... disconnecting ...");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.Log("Disconnected! (Reason " + cause + ")");
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("Received event " + photonEvent.Code);

        switch (photonEvent.Code)
        {}
    }
}
