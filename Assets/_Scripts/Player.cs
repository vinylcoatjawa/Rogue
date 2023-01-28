using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Transform _entityTransform;
    Vector3 _mouseWorldPos;
    Vector3 _mousePos;
    Vector3 _worldPos;
    PlayerStateManager playerStateManager;
    PlayerBaseState _currentState;
    [SerializeField] private VoidEvent OnPlayerEnterIdleState;
    [SerializeField] private Vector3Event OnSelectTargetTile;
    
    void Awake(){
        _entityTransform = gameObject.transform;
        playerStateManager = GetComponent<PlayerStateManager>();
        InputActions inputActions = new InputActions();
        inputActions.InputForTesting.Enable();
        inputActions.InputForTesting.Leftclick.performed += SetPlayerReadyToMove;
        inputActions.InputForTesting.Rightclick.performed += SetPlayerIdle;
    }
    /// <summary>
    /// Sets the player into idle state
    /// </summary>
    /// <param name="context">Currently not used</param>
    void SetPlayerIdle(InputAction.CallbackContext context){
        playerStateManager.SwitchState(playerStateManager.PlayerIdleState);
        _currentState = playerStateManager.PlayerIdleState;
        OnPlayerEnterIdleState.Raise();
    }
    /// <summary>
    /// Sets the player into pre move state where we can choose a tile t move to
    /// </summary>
    /// <param name="context">Currently not used</param>
    void SetPlayerReadyToMove(InputAction.CallbackContext context){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit);
        if(raycastHit.transform != null && raycastHit.transform.gameObject.CompareTag("Player")){
            playerStateManager.SwitchState(playerStateManager.PlayerPlanMoveState);
            _currentState = playerStateManager.PlayerPlanMoveState;
        }
    }



}
