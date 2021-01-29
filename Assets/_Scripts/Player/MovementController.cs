using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    #region Unity References
    [SerializeField, Range(0.0f, 1.0f)] private float myLerpFactor = 0.5f;
    [SerializeField] private float myMovementSpeed = 5.0f;
    [SerializeField] private float myRotationSpeed = 60.0f;
    #endregion

    #region Variables - Private
    private CharacterController myCharController = default;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        myCharController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 localDeltaMovement = transform.forward * ver;
        localDeltaMovement.Normalize();
        localDeltaMovement *= myMovementSpeed;

        Quaternion rotation = Quaternion.AngleAxis(hor * myRotationSpeed * Time.deltaTime, Vector3.up);
        transform.rotation *= rotation;

        myCharController.SimpleMove(localDeltaMovement);
    }
    #endregion
}
