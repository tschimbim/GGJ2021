using UnityEngine;

public class UIEmoteReceiverScript : MonoBehaviour
{
    public EmoteIconScript myEmoteIconPrefab = default;
    public float myDestroyIconDelay = 3.0f;
    public int myMaxIconAmount = 5;

    public void PushEmote(Emote emote)
    {
        EmoteIconScript emoteIcon = Instantiate(myEmoteIconPrefab, transform);
        emoteIcon.SetEmote(emote);
        emoteIcon.transform.SetAsFirstSibling();

        Destroy(emoteIcon.gameObject, myDestroyIconDelay);

        for (int i = transform.childCount - 1; i >= myMaxIconAmount; --i)
            Destroy(transform.GetChild(i).gameObject);
    }
}
