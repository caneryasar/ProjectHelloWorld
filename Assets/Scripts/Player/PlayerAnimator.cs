using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private EventArchive _eventArchive;

    private Animator _animator;
    private static readonly int MoveInput = Animator.StringToHash("MoveInput");
    private static readonly int SprintInput = Animator.StringToHash("SprintInput");
    private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int DashInput = Animator.StringToHash("DashInput");
    private static readonly int ComboStep = Animator.StringToHash("ComboStep");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    private void Awake() {

        _animator = GetComponent<Animator>();
        
        Subscribe();
    }

    private void Start() {
        
        ResetCombos();
    }

    private void Subscribe() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();

        _eventArchive.OnMoveInput += SetMove;
        _eventArchive.OnDoubleJump += SetDoubleJump;
        _eventArchive.OnSprintInput += SetSprint;
        _eventArchive.OnJumpInput += SetJump;
        _eventArchive.OnResetJump += ResetJump;
        _eventArchive.OnDashTriggered += TriggerDash;
        _eventArchive.OnAttackBegin += SetAttack;
        _eventArchive.OnSecondAttack += Trigger2ndCombo;
        _eventArchive.OnThirdAttack += Trigger3rdCombo;
        _eventArchive.OnComboBroken += ResetCombos;
        _eventArchive.OnAttackCountChange += ChangeCombo;
        _eventArchive.OnDirectionChangeFocused += GetDirection;
        _eventArchive.OnFocusHold += SetMoveFocused;
        _eventArchive.OnEnemyHitPlayer += GetHit;
        _eventArchive.OnCounterTriggered += SetCounter;
    }

    private void SetCounter(int counter) {
        
        _animator.SetBool("isCountering", true);
        _animator.SetInteger("CounterType", counter);

        DOVirtual.DelayedCall(1f, () => {
            
            _animator.SetInteger("CounterType", 0);
            _animator.SetBool("isCountering", false);
        });
    }

    private void GetHit() {
        
        _animator.SetLayerWeight(1, 1);

        DOVirtual.DelayedCall(1f, () => { _animator.SetLayerWeight(1, 0); });
    }

    private void SetMoveFocused(bool focused) {

        _animator.SetBool("Focus", focused);
    }

    private void GetDirection(Vector2 direction) {
        
        _animator.SetFloat("DirectionX", direction.x);
        _animator.SetFloat("DirectionZ", direction.y);
    }

    private void ChangeCombo(int count) {
        
        _animator.SetInteger(ComboStep, count);       
    }

    private void Trigger2ndCombo() {
          
        _animator.SetBool("Combo2", true);

    }

    private void Trigger3rdCombo() {
        
        _animator.SetBool("Combo3", true);

    }

    private void SetAttack() {

        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("NotAttackable")) { return; }
        
        _animator.SetBool(IsAttacking, true);
    }

    private void ResetCombos() {
        
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("Combo2", false);
        _animator.SetBool("Combo3", false);
    }

    private void TriggerDash() {
        
        _animator.SetTrigger(DashInput);
    }

    private void SetDoubleJump(bool obj) {
        
        _animator.SetTrigger(DoubleJump);
    }

    private void SetJump(bool isJumping) {
        
        _animator.SetBool(Jump, isJumping);
    }

    private void ResetJump() {
        
        _animator.SetBool(Jump, false);
        _animator.ResetTrigger(DoubleJump);
    }

    private void SetSprint(bool input) {
        
        _animator.SetBool(SprintInput, input);
    }

    private void SetMove(Vector2 input) {

        if(input.x != 0 || input.y != 0) {
            
            _animator.SetBool(MoveInput, true);
            
            return;
        }
        
        _animator.SetBool(MoveInput, false);
    }

    private IEnumerator ResetAnim(float timer, string type, int animId) {

        var resetTimer = 0f;
        
        while(resetTimer < timer) {
            
            resetTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(timer);

        switch(type) {

            case "float": {
                
                _animator.SetFloat(animId, 0f);
                break;
            } 
            case "trigger": {
                
                _animator.ResetTrigger(animId);
                break;
            } 
            case "int": {
                
                _animator.SetInteger(animId, 0);
                break;
            } 
            case "bool": {
                
                _animator.SetBool(animId, false);
                break;
            } 
        }
    }
}