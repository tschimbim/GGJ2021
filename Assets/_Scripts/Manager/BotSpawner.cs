using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class BotSpawner : MonoBehaviour
{
    public GameObject botPrefab;
    public int desiredAmount = 20;
    public float maxDistance = 60;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager networkManager = (NetworkManager)FindObjectOfType(typeof(NetworkManager));
        if (networkManager.IsHost())
        {
            SpawnBots();
        }
    }

    private void SpawnBots()
    {
        for (int i = 0; i < desiredAmount; ++i)
        {
            // Get Random Point inside Sphere which position is center, radius is maxDistance
            Vector3 randomPos = Random.insideUnitSphere * maxDistance + transform.position;

            NavMeshHit hit; // NavMesh Sampling Info Container
            // from randomPos find a nearest point on NavMesh surface in range of maxDistance
            NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

            PhotonNetwork.Instantiate("MPPrefabs/" + botPrefab.name, hit.position, Quaternion.Euler(0, Random.Range(0,360), 0), 0);
        }
    }
}
