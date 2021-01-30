using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.Linq;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public int mySpawnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        if (networkManager.IsInRoom())
            CreatePlayer();
    }

    void CreatePlayer()
    {
        if (PhotonNetwork.PlayerList.Length <= mySpawnIndex)
            return;

        if (PhotonNetwork.PlayerList[mySpawnIndex].IsLocal)
            PhotonNetwork.Instantiate("MPPrefabs/" + PlayerPrefab.name, transform.position, Quaternion.identity, 0);

        Destroy(gameObject);
    }
}
