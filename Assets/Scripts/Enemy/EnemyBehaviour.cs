using System;
using System.Collections;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour {
    
    // Components
    internal Animator animator;
    internal NavMeshAgent navMeshAgent;
    internal PlayerController player;

    // State Machine
    private IEnemyState currentState;

    // Flags
    internal bool isDead;
    internal bool isHit;
    internal bool canAttack;
    internal bool isAttacking;
    internal bool countered;
    internal bool finished;

    // Internal State
    private EventArchive _eventArchive;
    private bool _isPlayable;
    private bool _canProcess;

    [SerializeField] private int _health = 3;

    private void Awake() {
        
        // Event subscriptions
        _eventArchive = FindAnyObjectByType<EventArchive>();
        _eventArchive.OnPlayable += playable => _isPlayable = playable;
        _eventArchive.OnPlayerHitEnemy += GotHit;
    }

    private void OnEnable() {
        
        ResetEnemy();
        
        if(_canProcess) {
            
            TransitionToState(new IdleState());
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

        Debug.Log($"state: {currentState}");
        
        currentState?.UpdateState(this); // Delegate behavior to the current state
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
        
        currentState?.ExitState(this); // Exit the current state
        currentState = newState; // Set the new state
        currentState.EnterState(this); // Enter the new state
    }

    // Utility Methods Used by States
    public void LookAtPlayer() {
        
        var lookPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookPos);
    }

    public bool ArrivedAtPosition(Vector3 target) {
        
        return Vector3.Distance(transform.position, target) < 0.5f;
    }

    public Vector3 GenerateTargetPosition() {
        
        var randomPoint = player.transform.position + Random.insideUnitSphere * 5f;
        return NavMesh.SamplePosition(randomPoint, out var hit, 3f, NavMesh.AllAreas) ? hit.position : Vector3.zero;
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
            }
        }
    }

    internal void Dead() {
        
        _canProcess = false;
        // Additional cleanup if needed
    }

    internal void LowerHealth() {
        
        _health--;

        if(_health <= 0) {
            
            finished = player.inFinisher;
            countered = player.inCounter;
            isDead = true;
        }
    }

    private void ResetEnemy() {
        
        _health = 3;
        isDead = false;
        isHit = false;
        finished = false;
        countered = false;
        _canProcess = false;
    }
}