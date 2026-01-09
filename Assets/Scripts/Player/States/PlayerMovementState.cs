using UnityEngine;

public class PlayerMovementState : PlayerStateBase
{
    public PlayerMovementState(PlayerStateContext _context, PlayerStateMachine.PlayerStates _state) : base(_context, _state)
    {
    }

    public override void EnterState()
    {
        
    }
    
    public override void UpdateState()
    {
        context.SetMoveValue(context.GetInput().Player.Movement.ReadValue<Vector2>());
        context.CheckForGround();
    }

    public override void LateUpdateState()
    {
    }

    public override void FixedUpdateState()
    {
        Movement();
    }
    
    public override PlayerStateMachine.PlayerStates GetNextStateKey()
    {
        if (context.GetInput().Player.Jump.triggered && context.IsGrounded())
        {
            return PlayerStateMachine.PlayerStates.Jump;
        }
        
        return StateKey;
    }
    
    public override void ExitState()
    {
    }

}
