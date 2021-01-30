using System.Collections.Generic;
using UnityEngine;

public class UIEmoteSenderScript : MonoBehaviour
{
    public EmoteButtonScript myEmoteButtonPrefab = default;

    private void Start()
    {
        FillEmoteList();
    }

    public void FillEmoteList()
    {
        foreach (InGameMenu.EmoteData data in (GameManager.instance.localIsGhost ? InGameMenu.instance.myEmoteDataSender : InGameMenu.instance.myEmoteDataReceiver))
        {
            EmoteButtonScript emoteButton = Instantiate(myEmoteButtonPrefab, transform);
            emoteButton.SetEmote(data.emote, data.sprite);
        }
    }
}
