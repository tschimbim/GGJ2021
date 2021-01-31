using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SwitchSceneIfRoomFull : MonoBehaviour
{
    public int targetScene = -1;
    private bool startedRoomSwitch = false;

    // Update is called once per frame
    void Update()
    {
        if (startedRoomSwitch)
        {
            return;
        }

        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        if (networkManager.IsMpReady() && networkManager.IsHost())
        {
            startedRoomSwitch = true;

            Transition transition = (Transition)FindObjectOfType(typeof(Transition));
            transition.StartTransition(false, 0.6f);

            Invoke("DoChangeLevel", 0.6f);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        startedRoomSwitch = true;

        Transition transition = (Transition)FindObjectOfType(typeof(Transition));
        transition.StartTransition(false, 0.6f);

        Invoke("DoChangeLevel", 0.6f);
    }

}
