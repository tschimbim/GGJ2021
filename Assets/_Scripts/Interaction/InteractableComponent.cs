using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public class InteractableComponent : MonoBehaviour
{
    //#region Unity References
    //[SerializeField] private Collider myCollider = default;
    //#endregion

    #region Public Methods
    private void Start()
    {
        GetComponent<Outline>().enabled = false;
    }
    #endregion
}
