using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState _currentState;
    [SerializeField] private PlayerStateEvent OnPlayerStateChanged;
    public PlayerIdleState PlayerIdleState = new PlayerIdleState();
    public PlayerPlanMoveState PlayerPlanMoveState = new PlayerPlanMoveState();

    // Start is called before the first frame update
    void Start()
    {
        _currentState = PlayerIdleState;
        _currentState.EnterState(this);
        OnPlayerStateChanged.Raise(_currentState);
        //Debug.Log(_currentState);
    }


    public void SwitchState(PlayerBaseState state){
        _currentState = state;
        state.EnterState(this);
        OnPlayerStateChanged.Raise(_currentState);
        //Debug.Log(_currentState);
    }

  
}
