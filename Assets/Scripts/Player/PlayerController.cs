using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private EventArchive _eventArchive;

    private CharacterController _cc;

    private float _moveSpeed;
    
    private Vector2 _moveInput;    
    private Vector2 _lookInput;

    private bool _isSprinting;
    private bool _isJumping;
    private bool _isAttacking;
    
    //todo: invoke player specific methods from eventarchive
    
    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();

        _eventArchive.OnMoveInput += GetMoveInput;
        _eventArchive.OnLookInput += GetLookInput;
        _eventArchive.OnSprintInput += GetSprintInput;
        _eventArchive.OnJumpInput += GetJumpInput;
        _eventArchive.OnAttackInput += GetAttackInput;

        _cc = GetComponent<CharacterController>();
    }

    private void Start() {
        
        
    }

    private void Update() {

        _cc.Move(_moveInput);
    }

    private void GetAttackInput(bool receivedInput) { _isAttacking = receivedInput; }

    private void GetJumpInput(bool receivedInput) { _isJumping = receivedInput; }

    private void GetSprintInput(bool receivedInput) { _isSprinting = receivedInput; }

    private void GetLookInput(Vector2 receivedInput) { _lookInput = receivedInput; }

    private void GetMoveInput(Vector2 receivedInput) { _moveInput = receivedInput; }


}