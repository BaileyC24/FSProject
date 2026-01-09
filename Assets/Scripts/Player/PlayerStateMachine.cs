using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Sprint,
        Slide
    }
    
    private PlayerStateContext context;
    private PlayerInput playerInput;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rBody;

    public override void StartMethod()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput = new PlayerInput();
        playerInput.Enable();
        SetupContext();
        SetupState();
    }

    public override void UpdateMethod()
    {
    }
    
    private void SetupState()
    {
        States.Add(PlayerStates.Idle, new PlayerIdleState(context, PlayerStates.Idle));
        States.Add(PlayerStates.Fall, new PlayerFallState(context, PlayerStates.Fall));
        States.Add(PlayerStates.Jump, new PlayerJumpState(context, PlayerStates.Jump));
        
        CurrentState = States[PlayerStates.Idle];
    }
    
    private void SetupContext()
    {
        context = new PlayerStateContext(
            speed,
            playerInput,
            rBody,
            transform,
            Camera.main);
    }
}
