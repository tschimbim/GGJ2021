using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EmoteButtonScript : MonoBehaviour
{
    public Image myEmoteImage = default;

    private Emote myEmote = default;

    public void SetEmote(Emote emote, Sprite sprite)
    {
        myEmote = emote;
        myEmoteImage.sprite = sprite;
    }

    public void SendEmote()
    {
        GameManager.instance.photonView.RPC(nameof(GameManager.OnEmoteSent), RpcTarget.All, myEmote);
    }
}
