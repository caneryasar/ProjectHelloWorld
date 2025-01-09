using System;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private EventArchive _eventArchive;

    private CharacterController _charCon;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _dashDuration = 2f;
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private float _attackComboTime = 1f;
    [SerializeField] private float _attackComboTimeAlt = 1f;
    [SerializeField] private float _attackCounterTime = 1f;
    
    private Vector3 _moveInput;    
    private Vector2 _lookInput;
    private float _verticalVelocity;

    private bool _isSprinting;
    private bool _isJumping;
    private bool _isAttacking;
    private bool _is2ndCombo;
    private bool _is3rdCombo;
    private bool _doubleJumpable;
    private bool _isGrounded;
    private bool _isDashing;

    private float _defaultSpeed;
    private float _runSpeed;

    private float _dashTime = 0f;
    private float _dashSpeed;

    private int _sprintTriggered;
    private int _jumpTriggered;
    private int _attackTriggered;

    private float _attackTime;
    private float _attackMoveToTargetSpeed;

    private const float Gravity = -9.81f;
    
    //todo: invoke player specific methods from eventArchive
    
    private void Awake() {

        if(_eventArchive == null) { Subscribe(); }

        _charCon = GetComponent<CharacterController>();
    }

    private void Subscribe() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();

        _eventArchive.OnMoveInput += GetMoveInput;
        _eventArchive.OnSprintInput += GetSprintInput;
        _eventArchive.OnJumpInput += GetJumpInput;
        _eventArchive.OnAttack += () => _isAttacking = true;
        _eventArchive.On2ndAttack += () => _is2ndCombo = true;
        _eventArchive.On3rdAttack += () => _is3rdCombo = true;
        _eventArchive.OnJumpTriggered += () =>  _jumpTriggered++;
        _eventArchive.OnDashTriggered += () => _isDashing = true;
    }

    private void Start() {
        
        if(_eventArchive == null) { Subscribe(); }

        _defaultSpeed = _moveSpeed;
        _runSpeed = _defaultSpeed * 2f;
        _dashSpeed = _dashDistance / _dashDuration;

        this.ObserveEveryValueChanged(_ => _attackTriggered).Subscribe(_ => {

            _eventArchive.InvokeOnAttackCountChange(_attackTriggered);
        });
    }

    private void Update() {
        
        //todo: make forward based on camera look

        if(_isAttacking && !_isDashing) {
            
            //todo:find directed or selected enemy and move towards it

            // var fakeDistance = 7f;

            // _charCon.Move(transform.forward * (fakeDistance * Time.deltaTime));

            Debug.Log($"combo: {_attackTriggered} / 2nd: {_is2ndCombo} / 3rd: {_is3rdCombo}");
            
            if(_attackTriggered == 0) {

                _attackTriggered = 1;
            }
            
            if(_attackTime < _attackComboTime) {

                if(_is2ndCombo && _attackTriggered == 1) {

                    _attackTriggered = 2;
                    
                    _attackTime = 0;
                    
                    _eventArchive.InvokeOnSecondAttack();

                    return;
                }

                if(_is3rdCombo && _attackTriggered == 2) {

                    _attackTriggered = 3;
                    
                    _attackTime = 0;
                    
                    _eventArchive.InvokeOnThirdAttack();

                    
                    return;
                }

                _attackTime += Time.deltaTime;
                
                return;
            }
            
            _eventArchive.InvokeOnComboBroken();
            
            _isAttacking = false;
            _is2ndCombo = false;
            _is3rdCombo = false;
            _attackTriggered = 0;
            _attackTime = 0f;
            
            return;
        }

        _isAttacking = false;
        _attackTriggered = 0;
        
        if(_isDashing && _isGrounded && _moveInput != Vector3.zero) {

            if(_dashTime < _dashDuration) {
                
                _charCon.Move(transform.forward * (_dashSpeed * Time.deltaTime));

                _dashTime += Time.deltaTime;
                
                return;
            }
            
            _isDashing = false;

            _dashTime = 0f;
        }

        _isGrounded = _moveInput != Vector3.zero ? _charCon.isGrounded : Physics.Raycast(transform.position, -transform.up, _charCon.height * .05f);
        
        _moveSpeed = _isSprinting ? _runSpeed : _defaultSpeed;
        
        _charCon.Move(_moveInput * (Time.deltaTime * _moveSpeed));

        if(_moveInput != Vector3.zero) {

            var currentFwd = transform.forward;
            transform.forward = Vector3.SlerpUnclamped(currentFwd, _moveInput, _rotationSpeed * Time.deltaTime);
        }
        
        if(_isGrounded && _verticalVelocity < 0f) {
            
            _verticalVelocity = 0f;

            _jumpTriggered = 0;

            _doubleJumpable = false;
            
            _eventArchive.InvokeOnResetJump();
        }

        if(_isJumping && _isGrounded) {

            _verticalVelocity += Mathf.Sqrt(_jumpForce * -2f * Gravity);

            _doubleJumpable = true;
        }

        if(_isJumping && _doubleJumpable && _jumpTriggered > 0 && !_isGrounded) {
            
            _eventArchive.InvokeOnDoubleJump(true);
            
            _verticalVelocity += Mathf.Sqrt(_jumpForce * -2f * Gravity);
            
            _doubleJumpable = false;
        }

        _verticalVelocity += Gravity * Time.deltaTime;
        var moveUpwards = new Vector3(0, _verticalVelocity, 0);
        _charCon.Move(moveUpwards * Time.deltaTime);

        _isDashing = false;
    }

    private void GetJumpInput(bool receivedInput) { _isJumping = receivedInput; }

    private void GetSprintInput(bool receivedInput) { _isSprinting = receivedInput; }

    private void GetMoveInput(Vector2 receivedInput) { _moveInput = new Vector3(receivedInput.x, 0, receivedInput.y); }

    
}