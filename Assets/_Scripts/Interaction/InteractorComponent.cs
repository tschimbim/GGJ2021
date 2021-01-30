using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractorComponent : MonoBehaviourPun
{
    #region Properties - Public
    /// <summary>
    /// List of all nearby interactable objects
    /// </summary>
    public List<InteractableComponent> interactableList { get; } = new List<InteractableComponent>();
    private InteractableComponent myActiveInteractable = default;
    #endregion

    #region Unity References
    [SerializeField] private KeyCode myInteractionKeyCode = KeyCode.E;
    [SerializeField] private bool myOutlineTarget = true;
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        if (!photonView.IsMine)
            return;

        UpdateActiveInteractable();

        if (!(myActiveInteractable is null))
        {
            if (Input.GetKeyDown(myInteractionKeyCode))
                Interact(myActiveInteractable);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableComponent interactableComponent = other.GetComponent<InteractableComponent>();
        if (interactableComponent is null || interactableComponent.gameObject == gameObject)
            return;

        interactableList.Add(interactableComponent);
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableComponent interactableComponent = other.GetComponent<InteractableComponent>();
        if (interactableComponent is null || interactableComponent.gameObject == gameObject)
            return;

        interactableList.Remove(interactableComponent);
    }
    #endregion

    #region Methods - Private
    private void UpdateActiveInteractable()
    {
        var oldActiveInteractable = myActiveInteractable;

        // Deactivate outline for old active object
        if (myOutlineTarget && !(oldActiveInteractable is null))
        {
            Outline compOutline = oldActiveInteractable.GetComponent<Outline>();
            if (!(compOutline is null))
                compOutline.enabled = false;
        }

        if (interactableList.Count == 0)
        {
            myActiveInteractable = null;
            return;
        }

        // Set it active to closest interactable
        myActiveInteractable = interactableList.OrderBy(interactable => (interactable.transform.position - transform.position).sqrMagnitude).First();

        Outline outline = myActiveInteractable.GetComponent<Outline>();
        if (myOutlineTarget && !(outline is null))
            outline.enabled = true;
    }

    private void Interact(InteractableComponent interactable)
    {
        if (interactable.tag == "Bot" && interactable.gameObject == GameManager.instance.targetBot)
        {
            GameManager.instance.photonView.RPC(nameof(GameManager.RegisterBotCatch), RpcTarget.MasterClient, photonView.ViewID);
        }
    }
}
#endregion
