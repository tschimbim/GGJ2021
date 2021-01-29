﻿using System.Collections;
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

        NetworkManager networkManager = (NetworkManager) FindObjectOfType(typeof(NetworkManager));
        if (networkManager.IsMpReady() && networkManager.IsHost())
        {
            PhotonNetwork.LoadLevel(targetScene);
            startedRoomSwitch = true;
        }
    }
}