using UnityEngine;
using UnityEngine.UI;

public class EmoteIconScript : MonoBehaviour
{
    public Image myEmoteImage = default;

    public void SetEmote(Emote emote)
    {
        myEmoteImage.sprite = InGameMenu.instance.emoteDict[emote];
    }
}
