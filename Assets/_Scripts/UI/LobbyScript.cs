using Photon.Pun;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
    public GameObject myStartButton = default;

    private void Update()
    {
        myStartButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}
