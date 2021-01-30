using Photon.Pun;
using System;
using System.Linq;
using UnityEngine;

public enum GameState
{
    MainMenu,
    PreGame,
    InGame,
    PostGame
}

public class GameManager : SingletonPUN<GameManager>, IPunObservable
{
    #region Properties
    /// <summary>
    /// The amount of time for preparation left before the game actually starts
    /// </summary>
    public float pregameCooldown => myPregameCooldown;
    /// <summary>
    /// The time left until the game is over
    /// </summary>
    public float ingameTimeLeft => myIngameTimeLeft;

    /// <summary>
    /// The player that won the game
    /// </summary>
    public GameObject winningPlayer => myWinningPlayerViewId >= 0 ? PhotonNetwork.GetPhotonView(myWinningPlayerViewId).gameObject : null;

    public GameObject targetBot { get; private set; } = default;

    public bool localIsGhost => PhotonNetwork.IsMasterClient;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        OnGameStateChange(myGameState, GameState.PreGame);
    }

    private void Update()
    {
        switch (myGameState)
        {
            case GameState.MainMenu: UpdateMainMenu(); break;
            case GameState.PreGame: UpdatePreGame(); break;
            case GameState.InGame: UpdateInGame(); break;
            case GameState.PostGame: UpdatePostGame(); break;
        }
    }
    #endregion

    #region Methods - Public
    public void StartGame()
    {
        SetGameState(GameState.PreGame);
    }

    [PunRPC]
    public void RegisterBotCatch(int viewID)
    {
        myWinningPlayerViewId = viewID;

        SetGameState(GameState.PostGame);
    }
    [PunRPC]
    public void SetTargetBot(int viewID)
    {
        targetBot = PhotonNetwork.GetPhotonView(viewID).gameObject;
    }
    #endregion

    #region Methods - Private
    private void StartMainMenu() => StartGame();
    private void StartPreGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            myPreGameStartedTimestamp = Time.time;
            myWinningPlayerViewId = -1;
        }

        InGameMenu.instance?.ShowUI(GameState.PreGame);
    }
    private void StartInGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            myGameStartedTimestamp = Time.time;
        }

        InGameMenu.instance?.ShowUI(GameState.InGame);
    }
    private void StartPostGame()
    {
        InGameMenu.instance?.ShowUI(GameState.PostGame);

        Debug.LogFormat("GAME UPDATE: Player with id {0} won!", myWinningPlayerViewId);
    }

    private void StopMainMenu() { }
    private void StopPreGame() { }
    private void StopInGame() { }
    private void StopPostGame() { }

    private void UpdateMainMenu() { }
    private void UpdatePreGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            myPregameCooldown = Mathf.Clamp((myPreGameStartedTimestamp + myPreGameCooldown) - Time.time, 0, myPreGameCooldown);

            if (myPregameCooldown <= 0)
                SetGameState(GameState.InGame);
        }
    }
    private void UpdatePostGame()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    myPostGameTimeLeft = Mathf.Clamp((myGameStartedTimestamp + myGameDuration + myPostGameCooldown) - Time.time, 0, myGameDuration);

        //    if (myPostGameTimeLeft <= 0.0f)
        //        PhotonNetwork.LoadLevel(1);
        //}
    }
    private void UpdateInGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            myIngameTimeLeft = Mathf.Clamp((myGameStartedTimestamp + myGameDuration) - Time.time, 0, myGameDuration);

            if (myIngameTimeLeft <= 0.0f)
            {
                SetGameState(GameState.PostGame);
            }
        }
    }

    private void SetGameState(GameState gameState) => SetGameState(myGameState, gameState);
    private void SetGameState(GameState prevGameState, GameState nextGameState)
    {
        photonView.RPC(nameof(OnGameStateChange), RpcTarget.All, prevGameState, nextGameState);
    }

    [PunRPC]
    private void OnGameStateChange(GameState prevGameState, GameState nextGameState)
    {
        Debug.LogFormat("Game state change: {0} -> {1}", prevGameState.ToString(), nextGameState.ToString());
        myGameState = nextGameState;

        switch (prevGameState)
        {
            case GameState.MainMenu: StopMainMenu(); break;
            case GameState.PreGame: StopPreGame(); break;
            case GameState.InGame: StopInGame(); break;
            case GameState.PostGame: StopPostGame(); break;
        }

        switch (nextGameState)
        {
            case GameState.MainMenu: StartMainMenu(); break;
            case GameState.PreGame: StartPreGame(); break;
            case GameState.InGame: StartInGame(); break;
            case GameState.PostGame: StartPostGame(); break;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myPreGameCooldown);
            stream.SendNext(myIngameTimeLeft);
            stream.SendNext(myPostGameTimeLeft);
            stream.SendNext(myPreGameStartedTimestamp);
            stream.SendNext(myGameStartedTimestamp);
            stream.SendNext(myWinningPlayerViewId);
        }
        else
        {
            myPreGameCooldown = (int)stream.ReceiveNext();
            myIngameTimeLeft = (float)stream.ReceiveNext();
            myPostGameTimeLeft = (float)stream.ReceiveNext();
            myPreGameStartedTimestamp = (float)stream.ReceiveNext();
            myGameStartedTimestamp = (float)stream.ReceiveNext();
            myWinningPlayerViewId = (int)stream.ReceiveNext();
        }
    }
    #endregion

    #region Unity References
    [SerializeField] private int myPreGameCooldown = 3;
    [SerializeField] private int myGameDuration = 90;
    #endregion

    #region Variables

    /// <summary>
    /// The amount of time for preparation left before the game actually starts
    /// </summary>
    private float myPregameCooldown = 0.0f;
    /// <summary>
    /// The time left until the game is over
    /// </summary>
    private float myIngameTimeLeft = 0.0f;

    public float myPostGameTimeLeft = 0.0f;

    private GameState myGameState = GameState.MainMenu;

    private float myPreGameStartedTimestamp = 0.0f;
    private float myGameStartedTimestamp = 0.0f;

    private int myWinningPlayerViewId = -1;
    #endregion
}
