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

    [SerializeField, Range(0.0f, 1.0f)] private float mySeekerCamSize = 0.3f;
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

        target = FindObjectsOfType<BotScript>().RandomElement().gameObject;
        if (target != null)
        {
            target.gameObject.name = "BOT TARGET!";
            target.GetComponentInChildren<Renderer>().material = myTargetMaterial;

            Outline outline = target.GetComponent<Outline>();
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAll;
            GameManager.instance.photonView.RPC(nameof(GameManager.SetTargetBot), RpcTarget.All, target.GetPhotonView().ViewID);
        }
        else
        {
            Debug.LogWarning("Could not find target bot!");
            return;
        }

        otherPlayer = GameObject.FindGameObjectWithTag("Player");
        if (otherPlayer != null)
        {
            otherPlayer.GetComponentInChildren<Renderer>().material = mySeekerMaterial;
            Camera cam = otherPlayer.GetComponent<ActivateIfMine>().targetObject.GetComponent<Camera>();
            cam.rect = new Rect(cam.rect.position, Vector2.one * mySeekerCamSize);
            cam.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Could not find other player!");
            return;
        }

    }

}
