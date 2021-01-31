using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyStatusDisplayScript : MonoBehaviour
{
    private Text myText = default;

    private void Awake()
    {
        myText = GetComponent<Text>();
    }

    private void Update()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        myText.text = string.Format("CONNECTED PLAYERS {0}/{1}", PhotonNetwork.PlayerList.Length, networkManager.maxPlayersForRoom);
    }
}
