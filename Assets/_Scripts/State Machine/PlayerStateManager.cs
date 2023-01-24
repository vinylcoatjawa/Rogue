using UnityEngine;
/// <summary>
/// Class to handle state changes of the player character
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState _currentState;
    /// <summary>
    /// Event to be raised when the state of THIS is changed
    /// </summary>
    /// <returns></returns>
    [SerializeField] private PlayerStateEvent OnPlayerStateChanged;
    public PlayerIdleState PlayerIdleState = new PlayerIdleState();
    public PlayerPlanMoveState PlayerPlanMoveState = new PlayerPlanMoveState();

    void Start()
    {
        _currentState = PlayerIdleState;
        _currentState.EnterState(this);
        OnPlayerStateChanged.Raise(_currentState); // state change event raised in start
    }

    /// <summary>
    /// Switches the state of the Player
    /// </summary>
    /// <param name="state">The input state as a PlayerBaseState</param>
    public void SwitchState(PlayerBaseState state){
        _currentState = state;
        state.EnterState(this);
        OnPlayerStateChanged.Raise(_currentState);
    }

    

  
}
