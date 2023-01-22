using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    [SerializeField] private PlayerStateEvent OnPlayerStateChanged;
    public PlayerIdleState playerIdleState = new PlayerIdleState();
    public PlayerPlanMoveState playerPlanMoveState = new PlayerPlanMoveState();

    // Start is called before the first frame update
    void Start()
    {
        currentState = playerIdleState;
        currentState.EnterState(this);
        OnPlayerStateChanged.Raise(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(PlayerBaseState state){
        currentState = state;
        state.EnterState(this);
        OnPlayerStateChanged.Raise(currentState);
    }

  
}
