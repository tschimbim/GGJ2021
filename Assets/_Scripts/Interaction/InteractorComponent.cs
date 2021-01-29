using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractorComponent : MonoBehaviour
{
    #region Properties - Public
    /// <summary>
    /// List of all nearby interactable objects
    /// </summary>
    public List<InteractableComponent> interactableList { get; } = new List<InteractableComponent>();
    private InteractableComponent myActiveInteractable = default;
    #endregion

    #region Events
    public event Action onInteractableNearby;
    #endregion

    #region Unity References
    [SerializeField] private KeyCode myInteractionKeyCode = KeyCode.E;
    [SerializeField] private bool myOutlineTarget = true;
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        UpdateActiveInteractable();

        if (!(myActiveInteractable is null))
        {
            if (Input.GetKeyDown(myInteractionKeyCode))
                myActiveInteractable.Interact(this);
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

        if (myOutlineTarget)
            myActiveInteractable.GetComponent<Outline>().enabled = true;
    }
}
#endregion
