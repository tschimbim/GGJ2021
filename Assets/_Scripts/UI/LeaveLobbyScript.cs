using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveLobbyScript : MonoBehaviour
{
    public void LeaveLobby()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        networkManager.LeaveRoom();
    }
}
