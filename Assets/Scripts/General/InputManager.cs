using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    
    //Todo: setup input
    
    private EventArchive _eventArchive;

    private InputAction _move;
    private InputAction _look;
    private InputAction _sprint;
    private InputAction _jump;

    private bool _isPlayable;
    
    
    private void Awake() {

        _eventArchive = FindAnyObjectByType<EventArchive>();

        _move = InputSystem.actions.FindAction("Move");
        _look = InputSystem.actions.FindAction("Look");
        _sprint = InputSystem.actions.FindAction("Sprint");
        _jump = InputSystem.actions.FindAction("Jump");
    }

    private void Start() {
        
    }
    
    private void Update() {
        
        if(_isPlayable) {
            
            _eventArchive.InvokeOnMoveInput(_move.ReadValue<Vector2>());
            _eventArchive.InvokeOnLookInput(_look.ReadValue<Vector2>());
            _eventArchive.InvokeOnJumpInput(_jump.ReadValue<bool>());
            _eventArchive.InvokeOnSprintInput(_sprint.ReadValue<bool>());
        }
    }
}