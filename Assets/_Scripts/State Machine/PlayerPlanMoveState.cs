public class PlayerPlanMoveState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        //Debug.Log("Enetered move state");
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }
}
