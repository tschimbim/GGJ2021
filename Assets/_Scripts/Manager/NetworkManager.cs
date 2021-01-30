using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public byte maxPlayersForRoom = 2;

    private string gameVersion = "1";

    private NetworkState State = NetworkState.Offline;

    public override void OnEnable()
    {
        base.OnEnable();

        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool IsMpReady()
    {
        return PhotonNetwork.PlayerList.Length == maxPlayersForRoom;
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

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;


        State = NetworkState.Connecting;
        Debug.Log("... connecting ...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("... Connected!");
        State = NetworkState.Online;
        JoinRoom(System.Environment.UserName);
    }

    public bool IsInRoom()
    {
        return State == NetworkState.InRoom;
    }

    public NetworkState GetState()
    {
        return State;
    }

    public void JoinRoom(string roomName = null)
    {
        Debug.Log("Join Random Room ...");
        State = NetworkState.SearchingRoom;
        if (string.IsNullOrEmpty(roomName))
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        State = NetworkState.InRoom;
        Debug.Log("... Room created!");
    }

    public override void OnJoinedRoom()
    {
        State = NetworkState.InRoom;
        Debug.Log("... Room joined!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        State = NetworkState.Failed;
        Debug.Log("... Room creation failed!");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        State = NetworkState.Failed;
        Debug.Log("... Room join failed!");

#if UNITY_EDITOR
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayersForRoom;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(System.Environment.UserName, roomOptions, TypedLobby.Default);    //< create room for local user name
#else
        JoinRoom();
#endif
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("... Random Room join failed.");
        Debug.Log("Create Random Room ...");

        State = NetworkState.CreatingRoom;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayersForRoom;
        roomOptions.IsVisible = true;

#if UNITY_EDITOR
        PhotonNetwork.CreateRoom(System.Environment.UserName, roomOptions, TypedLobby.Default);    //< create room for local user name
#else
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);    //< create room, let server decide for the name
#endif
    }

    public void Disconnect()
    {
        Debug.Log("Network: Trying to disconnect...");
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("... not Connected");
        }

        PhotonNetwork.Disconnect();
        State = NetworkState.Offline;
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

    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        Debug.Log("Received event " + photonEvent.Code);
    }
}
