using Photon.Pun;
using System.Linq;
using UnityEngine;

public class GhostScript : MonoBehaviourPun
{
    #region Properties - Public
    public GameObject otherPlayer { get; private set; } = null;
    public GameObject target { get; private set; } = null;

    public bool isGhost => photonView.Owner == PhotonNetwork.MasterClient;
    #endregion

    #region Unity References
    [SerializeField] private Material mySeekerMaterial = default;
    [SerializeField] private Material myTargetMaterial = default;
    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        if (!isGhost)
        {
            Destroy(this);
            return;
        }

        Invoke("StartGhostView", 1.5f);
    }
    #endregion

    void StartGhostView()
    {
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        gameObject.tag = "Ghost";

        if (!photonView.AmOwner)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                renderer.enabled = false;

            Destroy(this);
            return;
        }

        otherPlayer = GameObject.FindGameObjectWithTag("Player");
        if (otherPlayer == null)
        {
            Debug.LogWarning("Could not find other player!");
            return;
        }

        otherPlayer.GetComponentInChildren<Renderer>().material = mySeekerMaterial;

        target = FindObjectsOfType<BotScript>().RandomElement().gameObject;
        target.GetComponentInChildren<Renderer>().material = myTargetMaterial;
        GameManager.instance.photonView.RPC(nameof(GameManager.SetTargetBot), RpcTarget.All, target.GetPhotonView().ViewID);
    }

}
