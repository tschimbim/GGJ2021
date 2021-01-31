using UnityEngine;

public class HostButtonScript : MonoBehaviour
{
    public void HostGame()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        networkManager.CreateRoom();
    }
}
