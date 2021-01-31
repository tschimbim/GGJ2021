using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour
{
    public int targetScene = 2;
    public Button myButton = default;

    private bool startedRoomSwitch = false;

    private void Update()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));

        myButton.interactable = !startedRoomSwitch && networkManager.IsMpReady();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        startedRoomSwitch = true;

        Transition transition = (Transition)FindObjectOfType(typeof(Transition));
        transition.StartTransition(false, 0.6f);

        Invoke(nameof(DoChangeLevel), 0.6f);
    }

    public void DoChangeLevel()
    {
        PhotonNetwork.LoadLevel(targetScene);
    }
}
