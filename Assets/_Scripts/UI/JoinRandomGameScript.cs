using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRandomGameScript : MonoBehaviour
{
    public void JoinRandomGame()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        networkManager.JoinRoom(System.Environment.UserName);
    }
}
