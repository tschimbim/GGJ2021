﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviourPun
{
    #region Unity References
    [SerializeField] private float myMovementSpeed = 5.0f;
    [SerializeField] private float myRotationSpeed = 60.0f;
    #endregion

    #region Properties - Public
    #endregion

    #region Variables - Private
    private CharacterController myCharController = default;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        myCharController = GetComponent<CharacterController>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
            return;

        if (GameManager.instance.gameState != GameState.InGame)
            return;

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
