using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NetworkStatusDisplay : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Text textDisplay = gameObject.GetComponent<Text>();

        NetworkManager networkManager = (NetworkManager) FindObjectOfType(typeof(NetworkManager));
        switch(networkManager.GetState())
        {
            case NetworkState.Failed:
                textDisplay.text = "Failed";
                break;
            case NetworkState.Offline:
                textDisplay.text = "Offline";
                break;
            case NetworkState.Connecting:
                textDisplay.text = "Connecting";
                break;
            case NetworkState.Online:
                textDisplay.text = "Online";
                break;
            case NetworkState.SearchingRoom:
                textDisplay.text = "Searching Room";
                break;
            case NetworkState.CreatingRoom:
                textDisplay.text = "Creating Room";
                break;
            case NetworkState.InRoom:
                textDisplay.text = "In Room";
                break;
        }
    }
}
