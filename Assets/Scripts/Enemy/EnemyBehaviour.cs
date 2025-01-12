using System;
using System.Collections;
using DG.Tweening;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour {
    
    internal Animator animator;
    internal NavMeshAgent navMeshAgent;
    internal PlayerController player;

    public Canvas indicatorCanvas;
    public Image indicator;

    private IEnemyState _currentState;

    internal bool isDead;
    internal bool isHit;
    internal bool canAttack;
    internal bool isAttacking;
    internal bool countered;
    internal bool finished;

    private EventArchive _eventArchive;
    private bool _isPlayable;
    private bool _canProcess;
    private bool _isSelected;

    public int health = 3;

    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
        _eventArchive.OnPlayable += playable => _isPlayable = playable;
        _eventArchive.OnPlayerHitEnemy += GotHit;
        _eventArchive.OnTargetAssigned += t => {
            
            if(t == transform) {
                
                _isSelected = true;
                indicator.gameObject.SetActive(true);
            }
            else {
                
                _isSelected = false;
                indicator.gameObject.SetActive(false); }
        };
    }

    private void OnEnable() {
        
        ResetEnemy();
        
        if(_canProcess) {
            
            TransitionToState(new IdleState());
            indicatorCanvas.gameObject.SetActive(true);
            indicator.gameObject.SetActive(false);
        }
    }

    private IEnumerator Start() {
        
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<PlayerController>();

        yield return new WaitForSeconds(1f);

        _canProcess = true;
        TransitionToState(new IdleState());
    }

    private void Update() {
        
        if(!_isPlayable || !_canProcess) return;
        
        _currentState?.UpdateState(this);

        if(isDead) {
            
            indicatorCanvas.gameObject.SetActive(false); 
            return;
        }

        indicatorCanvas.transform.LookAt(player.transform.position);

        if(isAttacking) {

            if(Vector3.Distance(transform.position, player.transform.position) < 3f) {

                indicator.color = Color.green;
                
                return;
            }
            
            indicator.color = Color.red;
            
            return;
        }

        if(_isSelected) {
            
            indicator.color = Color.yellow;
            return;
        }
        
        indicator.color = Color.white;
        
        
    }

    // Handle Hit Event
    private void GotHit(EnemyBehaviour hit) {
        
        if(hit != this) return;

        isHit = true;
        LowerHealth();

        DOVirtual.DelayedCall(0.5f, () => { isHit = false; }, false);
    }

    // State Management
    public void TransitionToState(IEnemyState newState) {
        
        _currentState?.ExitState(this); // Exit the current state
        _currentState = newState; // Set the new state
        _currentState.EnterState(this); // Enter the new state
    }

    // Utility Methods Used by States
    public void LookAtPlayer() {
        
        var lookPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookPos);
    }

    public bool ArrivedAtPosition(Vector3 target) {
        
        return Vector3.Distance(transform.position, target) < 0.5f;
    }
    
    internal Vector3 CalculateSphericalTargetPosition(float angle, float radius) {
        
        var angleRad = angle * Mathf.Deg2Rad;

        var offset = new Vector3(Mathf.Cos(angleRad) * radius, 0, Mathf.Sin(angleRad) * radius);

        return player.transform.position + offset;
    }

    public void UpdateAnimatorDirection(Vector3 target) {
        
        var worldDirection = (target - transform.position).normalized;
        var localDirection = transform.InverseTransformDirection(worldDirection);

        animator.SetFloat("DirectionZ", localDirection.z);
        animator.SetFloat("DirectionX", localDirection.x);
    }

    internal bool CloseToPlayer() {
        
        if(!player) { return false; }
        
        float distance = Vector3.Distance(transform.position, player.transform.position);

        float attackRange = 1.5f; 
        
        return distance <= attackRange;
    }


    internal void CheckForHit() {
        
        if(isDead) return;

        var origin = transform.position + (Vector3.up * navMeshAgent.height * 0.5f);
        
        if(Physics.SphereCast(origin, 0.5f, transform.forward * navMeshAgent.radius, out var raycastHit)) {
            
            if(raycastHit.transform.CompareTag("Player")) {
                
                _eventArchive.InvokeOnEnemyHitPlayer();

                raycastHit.transform.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            }
        }
    }

    internal void Dead() {
        
        _canProcess = false;
        // Additional cleanup if needed
    }

    internal void LowerHealth() {
        
        health--;

        if(health <= 0) {
            
            finished = player.inFinisher;
            countered = player.inCounter;
            isDead = true;
        }
    }

    private void ResetEnemy() {
        
        health = 3;
        isDead = false;
        isHit = false;
        finished = false;
        countered = false;
        _canProcess = false;
    }
}