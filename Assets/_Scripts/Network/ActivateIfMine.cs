using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivateIfMine : MonoBehaviourPun
{
    public GameObject targetObject = null;

    // Start is called before the first frame update
    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(photonView.IsMine);
        }
    }
}
