using UnityEditor;
using UnityEngine;

public abstract class PlayerStateBase : StateBase<PlayerStateMachine.PlayerStates>
{
    protected readonly PlayerStateContext context;
    protected PlayerStateBase(PlayerStateContext _context, PlayerStateMachine.PlayerStates _state) :
        base(_state)
    {
        context = _context;
    }

    protected void Movement()
    {
        Transform cameraTransform = context.GetCamera().transform;
        Vector2 input = context.GetMoveValue();
        Vector3 direction = cameraTransform.forward * input.y + cameraTransform.right * input.x;
        direction.y = 0;
        direction.Normalize();
        Vector3 targetPos = context.GetRb().position + (direction * (context.GetSpeed() * Time.fixedDeltaTime));

        context.GetRb().MovePosition(targetPos);
    }
}