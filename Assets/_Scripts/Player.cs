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
    [SerializeField] private VoidEvent OnPlayerEnterIdleState;
    
    void Awake(){
        _entityTransform = gameObject.transform;
        playerStateManager = GetComponent<PlayerStateManager>();
        InputActions inputActions = new InputActions();
        inputActions.InputForTesting.Enable();
        inputActions.InputForTesting.Leftclick.performed += ClickOnPlayer;
        inputActions.InputForTesting.Rightclick.performed += SetPlayerIdle;
    }

    private void SetPlayerIdle(InputAction.CallbackContext context){
        playerStateManager.SwitchState(playerStateManager.PlayerIdleState);
        OnPlayerEnterIdleState.Raise();
    }

    void ClickOnPlayer(InputAction.CallbackContext context){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit);
        if(raycastHit.transform != null && raycastHit.transform.gameObject.CompareTag("Player")){
            playerStateManager.SwitchState(playerStateManager.PlayerPlanMoveState);
        }
    }
    


}
