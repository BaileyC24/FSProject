using UnityEngine;

public class PlayerStateContext
{
    private float speed;
    private PlayerInput input;
    private Vector2 moveValue;
    private Rigidbody rBody;
    private Camera camera;
    private Transform playerTransform;
    private bool isGrounded;

    public PlayerStateContext(
        float _speed,
        PlayerInput _input,
        Rigidbody _rBody,
        Transform _playerTransform,
        Camera _camera)
    {
        speed = _speed;
        input = _input;
        rBody = _rBody;
        playerTransform = _playerTransform;
        camera = _camera;
    }

    #region Getters
    public float GetSpeed() => speed;
    public PlayerInput GetInput() => input;
    public Vector2 GetMoveValue() => moveValue;
    public bool CanMove() => GetMoveValue().magnitude > 0.01f;
    public Rigidbody GetRb() => rBody;
    public Camera GetCamera() => camera;
    public bool IsGrounded() => isGrounded;
    #endregion
    
    #region Setters

    public void SetMoveValue(Vector2 _value) => moveValue = _value;

    #endregion

    public void CheckForGround()
    {
        isGrounded = Physics.Raycast(playerTransform.position, -playerTransform.up,
            1.1f, ~LayerMask.GetMask("Player"));
        Debug.DrawRay(playerTransform.position, -playerTransform.up * 1.1f, Color.red);
    }


}