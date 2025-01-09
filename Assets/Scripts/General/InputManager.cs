using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    
    //Todo: setup input
    
    private EventArchive _eventArchive;

    private InputAction _move;
    private InputAction _look;
    private InputAction _sprint;
    private InputAction _jump;
    private InputAction _dash;
    private InputAction _attack;
    private InputAction _attackCombo2;
    private InputAction _attackCombo3;

    private bool _isPlayable;
    
    
    private void Awake() {

        _eventArchive = FindAnyObjectByType<EventArchive>();

        _move = InputSystem.actions.FindAction("Move");
        _look = InputSystem.actions.FindAction("Look");
        _sprint = InputSystem.actions.FindAction("Sprint");
        _jump = InputSystem.actions.FindAction("Jump");
        _dash = InputSystem.actions.FindAction("Dash");
        _attack = InputSystem.actions.FindAction("Attack");
        _attackCombo2 = InputSystem.actions.FindAction("AttackCombo2");
        _attackCombo3 = InputSystem.actions.FindAction("AttackCombo3");

        _eventArchive.OnPlayable += ChangePlayableStatus;
    }

    private void ChangePlayableStatus(bool isPlayable) {

        _isPlayable = isPlayable;
    }

    private void Start() {
        
    }
    
    private void Update() {
        
        if(!_isPlayable) { return; }
        
        // Debug.Log($"{_attack.triggered} / {_attackCombo2.triggered} / {_attackCombo3.triggered}");
            
        _eventArchive.InvokeOnMoveInput(_move.ReadValue<Vector2>());
        _eventArchive.InvokeOnJumpInput(_jump.IsPressed());
        if(_jump.triggered) { _eventArchive.InvokeOnJumpTriggered(); } 
        if(_dash.triggered) { _eventArchive.InvokeOnDashTriggered(); } 
        _eventArchive.InvokeOnSprintInput(_sprint.IsPressed());
        if(_attack.triggered) { _eventArchive.InvokeOnAttack(); }
        if(_attackCombo2.triggered) { _eventArchive.InvokeOn2ndAttack(); }
        if(_attackCombo3.triggered) { _eventArchive.InvokeOn3rdAttack(); }
    }
}