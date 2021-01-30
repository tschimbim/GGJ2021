using System.Collections.Generic;
using UnityEngine;

public class UIEmoteSenderScript : MonoBehaviour
{
    public EmoteButtonScript myEmoteButtonPrefab = default;

    private void Start()
    {
        FillEmoteList();

        gameObject.SetActive(GameManager.instance.localIsGhost);
    }

    public void FillEmoteList()
    {
        foreach (InGameMenu.EmoteData data in InGameMenu.instance.myEmoteData)
        {
            EmoteButtonScript emoteButton = Instantiate(myEmoteButtonPrefab, transform);
            emoteButton.SetEmote(data.emote, data.sprite);
        }
    }
}
