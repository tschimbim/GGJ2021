using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableComponent : MonoBehaviour
{
    //#region Unity References
    //[SerializeField] private Collider myCollider = default;
    //#endregion

    #region Public Methods
    public void Interact(InteractorComponent interactor)
    {

    }
    #endregion
}
