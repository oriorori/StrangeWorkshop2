using UnityEngine;
using UnityEngine.InputSystem;

// player의 직접적인 움직임에는 관여x
// input을 변환해주는 역할
public class InputHandler
 {
    private PlayerInput _playerInput;

    public Vector3 Movement
    {
        get => _movement;
    }

    private Vector3 _movement;
    
    public InputHandler(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        
        _playerInput.actions["Move"].performed += PressMoveInput;
        _playerInput.actions["Move"].canceled += ReleaseMoveInput;
    }
    
    private void PressMoveInput(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        _movement = new Vector3(movement.x, 0, movement.y);
    }

    private void ReleaseMoveInput(InputAction.CallbackContext ctx)
    {
        _movement = Vector3.zero;
    }

    public void Dispose()
    {
        _playerInput.actions["Move"].performed -= PressMoveInput;
    }
}
