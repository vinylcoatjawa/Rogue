using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState _currentState;
    [SerializeField] private PlayerStateEvent OnPlayerStateChanged;
    public PlayerIdleState PlayerIdleState = new PlayerIdleState();
    public PlayerPlanMoveState PlayerPlanMoveState = new PlayerPlanMoveState();

    void Start()
    {
        _currentState = PlayerIdleState;
        _currentState.EnterState(this);
        OnPlayerStateChanged.Raise(_currentState);
    }


    public void SwitchState(PlayerBaseState state){
        _currentState = state;
        state.EnterState(this);
        OnPlayerStateChanged.Raise(_currentState);
    }

    

  
}
