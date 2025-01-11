using System;
using System.Collections;
using Autodesk.Fbx;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


//todo: sprint/dash

public class PlayerController : MonoBehaviour {
    
    private EventArchive _eventArchive;
    
    public Transform playerModel;
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _dashDuration = 2f;
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private float _attackComboTime = .75f;
    [SerializeField] private float _attackComboTimeAlt = 1f; 
    [SerializeField] private float _attackCounterTime = 1f;
    
    private Transform _playerCam;
    [SerializeField] private CinemachineFreeLook playerCmFreeLook;
    [SerializeField] private CinemachineVirtualCamera playerCmVCam;
    private CharacterController _charCon;
    
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

    private bool _inDash;

    private bool _focusTriggered;
    private bool _isFocused;
    private Transform _target;

    private float _defaultSpeed;
    private float _runSpeed;

    private float _dashTime = 0f;
    private float _dashSpeed;

    private float _comboTimer = 0f;

    private int _sprintTriggered;
    private int _jumpTriggered;
    private int _attackTriggered;

    private float _attackTime;
    private float _attackMoveToTargetSpeed;

    private const float Gravity = -9.81f;
    
    
    private void Awake() {

        if(_eventArchive == null) { Subscribe(); }

        _charCon = GetComponent<CharacterController>();
        playerCmFreeLook = GetComponentInChildren<CinemachineFreeLook>();
        playerCmVCam = GetComponentInChildren<CinemachineVirtualCamera>();
        
    }

    private void Subscribe() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();

        _eventArchive.OnMoveInput += input => {

            _moveInput = Vector3.forward * input.y + Vector3.right * input.x;
        };
        _eventArchive.OnSprintInput += x => _isSprinting = x;
        _eventArchive.OnJumpInput += x => _isJumping = x;
        _eventArchive.OnAttack += () => _isAttacking = true;
        _eventArchive.On2ndAttack += () => _is2ndCombo = true;
        _eventArchive.On3rdAttack += () => _is3rdCombo = true;
        _eventArchive.OnJumpTriggered += () =>  _jumpTriggered++;
        _eventArchive.OnDashTriggered += () => _isDashing = true;
        _eventArchive.OnFocus += () => _focusTriggered = true;
        _eventArchive.OnFocusHold += holding => {
            
            _isFocused = holding;

            if(holding) {
                
                playerCmVCam.Priority = 10;
                playerCmFreeLook.Priority = 0;
            }
        };
        _eventArchive.OnTargetAssigned += t => {
            
            _target = t;
            playerCmVCam.LookAt = _target;

        };
        _eventArchive.OnResetCamTarget += () => {
            
            playerCmFreeLook.
            
            playerCmVCam.m_Priority = 0;
            playerCmFreeLook.Priority = 10;
        };
    }

    private void Start() {
        
        if(_eventArchive == null) { Subscribe(); }

        _defaultSpeed = _moveSpeed;
        _runSpeed = _defaultSpeed * 2f;
        _dashSpeed = _dashDistance / _dashDuration;

        _target = playerModel;

        // _playerCam = GetComponentInChildren<CinemachineFreeLook>().transform;
        if(Camera.main != null) _playerCam = Camera.main.transform;

        this.ObserveEveryValueChanged(_ => _attackTriggered).Subscribe(_ => {

            _eventArchive.InvokeOnAttackCountChange(_attackTriggered);
        });
    }

    private void Update() {

        _isGrounded = _moveInput != Vector3.zero ? _charCon.isGrounded : Physics.Raycast(transform.position + (Vector3.up * (_charCon.height * .5f)), -transform.up, _charCon.height * .6f);
        _moveSpeed = (_isSprinting && _isGrounded) ? _runSpeed : _defaultSpeed;
        
        var camFwd = _playerCam.forward;
        camFwd.y = 0;
        camFwd.Normalize();
        var camRight = _playerCam.right;
        camRight.y = 0;
        camRight.Normalize();
        
        var moveDirection = camFwd * _moveInput.z + camRight * _moveInput.x;

        
        if(_focusTriggered) {

            _eventArchive.InvokeOnTargetSearch(transform);
        }

        if(_isFocused) {
            
            transform.LookAt(_target);
        }
        
        if(_isAttacking && !_isDashing && _isGrounded) {

            if(_isFocused) {

                if(_target) {
                    
                    transform.DOMove(_target.position - (Vector3.forward + Vector3.right) * .65f, 1f);
                }
            }
            
            if(_attackTriggered == 0) {

                _attackTriggered = 1;

                _comboTimer = _attackComboTime;
            }
            
            
            if(_attackTime < _comboTimer) {

                if(_is2ndCombo && _attackTriggered == 1) {

                    _attackTriggered = 2;
                    
                    _attackTime = 0;

                    _comboTimer = _attackComboTimeAlt;
                    
                    _eventArchive.InvokeOnSecondAttack();

                    return;
                }

                if(_is3rdCombo && _attackTriggered == 2) {

                    _attackTriggered = 3;
                    
                    _attackTime = 0;

                    _comboTimer = _attackComboTimeAlt;
                    
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
            _comboTimer = 0f;
            
            return;
        }

        _isAttacking = false;
        _attackTriggered = 0;
        
        if(_isDashing && _isGrounded && _moveInput != Vector3.zero) {

            if(_inDash) { return; }

            transform.DOMove(transform.position + transform.forward * _dashDistance, _dashDuration).OnUpdate(() =>_inDash = true)
                .OnComplete(() => {
                    _inDash = false;
                    _isDashing = false;
                });
        }
        else {
            
            if(_inDash) { return; }
            _isDashing = false;
        }

        _charCon.Move(moveDirection * (Time.deltaTime * _moveSpeed));

        if(_moveInput != Vector3.zero && !_isFocused) {

            var currentFwd = transform.forward;
            transform.forward = Vector3.SlerpUnclamped(currentFwd, moveDirection, _rotationSpeed * Time.deltaTime);
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

    }

    
}