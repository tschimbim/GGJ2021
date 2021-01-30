using Photon.Pun;
using UnityEngine;

public class StartButtonScript : MonoBehaviour
{
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
