using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonScript : MonoBehaviour
{
    private bool startedRoomSwitch = false;

    private void Start()
    {
        gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void RestartGame()
    {
        startedRoomSwitch = true;

        Transition transition = (Transition)FindObjectOfType(typeof(Transition));
        transition.StartTransition(false, 0.6f);

        Invoke(nameof(DoChangeLevel), 0.6f);
    }

    public void DoChangeLevel()
    {
        PhotonNetwork.LoadLevel(0); // Main menu
    }
}
