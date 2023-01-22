using UnityEngine;

public class EventTester : MonoBehaviour
{
    PlayerBaseState _playerState;
    
    public void Display(PlayerBaseState state){ 
        Debug.Log(state);
    }
}
