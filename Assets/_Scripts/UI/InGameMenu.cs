using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : SingletonBase<InGameMenu>
{
    [System.Serializable]
    public class EmoteData
    {
        public Emote emote;
        public Sprite sprite;
    }

    public Dictionary<Emote, Sprite> emoteDict { get; private set; } = new Dictionary<Emote, Sprite>();

    public GameObject myPreGameUI = default;
    public GameObject myInGameUI = default;
    public GameObject myPostGameUI = default;

    public EmoteData[] myEmoteDataSender = default;
    public EmoteData[] myEmoteDataReceiver = default;
    public UIEmoteReceiverScript myEmoteReceiverScript = default;

    protected override void Awake()
    {
        base.Awake();

        foreach (EmoteData data in myEmoteDataSender)
            emoteDict.Add(data.emote, data.sprite);

        foreach (EmoteData data in myEmoteDataReceiver)
            emoteDict.Add(data.emote, data.sprite);
    }

    private void Start()
    {
        ShowUI(GameState.PreGame);
    }

    public void ShowUI(GameState gameState)
    {
        myPreGameUI.SetActive(gameState == GameState.PreGame);
        myInGameUI.SetActive(gameState == GameState.InGame);
        myPostGameUI.SetActive(gameState == GameState.PostGame);
    }

    public void PushEmote(Emote emote) => myEmoteReceiverScript.PushEmote(emote);

}
