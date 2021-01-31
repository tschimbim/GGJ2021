using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject myPreLobby = default;
    public GameObject myLobby = default;

    // Start is called before the first frame update
    void Start()
    {
        myPreLobby.SetActive(true);
        myLobby.SetActive(false);
    }

    private void Update()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));

        myPreLobby.SetActive(!networkManager.IsInRoom());
        myLobby.SetActive(networkManager.IsInRoom());
    }
}
