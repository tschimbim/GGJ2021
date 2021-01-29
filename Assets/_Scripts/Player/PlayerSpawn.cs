using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public bool host = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NetworkManager networkManager = (NetworkManager) FindObjectOfType(typeof(NetworkManager));
        if (networkManager.IsInRoom())
        {
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        NetworkManager networkManager = (NetworkManager) FindObjectOfType(typeof(NetworkManager));
        if (networkManager.IsHost() == host)
        {
            PhotonNetwork.Instantiate("MPPrefabs/" + PlayerPrefab.name, transform.position, Quaternion.identity, 0);
        }
        Destroy(gameObject);
    }
}
