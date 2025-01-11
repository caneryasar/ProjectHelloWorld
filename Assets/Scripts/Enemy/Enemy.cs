/*using System;
using System.Collections;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    
    internal Animator animator;

    internal NavMeshAgent navMeshAgent;

    private EventArchive _eventArchive;

    internal EnemyState enemyState;

    internal PlayerController player;

    private bool _isPlayable;

    internal bool canAttack;
    internal bool isAttacking;
    internal bool isHit;
    internal bool countered;
    internal bool finished;

    internal bool normalDeath;

    [SerializeField] private int _health = 3;

    internal bool isDead;

    private bool _canProcess;

    private void Awake() {

        _eventArchive = FindAnyObjectByType<EventArchive>();

        _eventArchive.OnPlayable += playable => _isPlayable = playable;
        _eventArchive.OnPlayerHitEnemy += GotHit;
    }

    private void GotHit(Enemy hit) {

        Debug.Log("enemy got hit");
        
        if(hit == this) {

            isHit = true;
            LowerHealth();
        }

        DOVirtual.DelayedCall(.5f, () => {

            isHit = false;
        }, false);
    }


    private void OnEnable() {

        _health = 3;
        
        if(!_canProcess){ return; }
        
        enemyState = null;
        enemyState = new Idle(this);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start() {

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<PlayerController>();

        yield return new WaitForSeconds(1f);

        _canProcess = true;
        
        enemyState = new Idle(this);

    }

    // Update is called once per frame
    void Update() {
        
        if(!_isPlayable || !_canProcess) { return; }
        
        enemyState = enemyState.Process();
    }

    internal void CheckForHit() {
        
        if(isDead) { return; }
        
        var origin = transform.position + (Vector3.up * navMeshAgent.height * .5f);
        
        if(Physics.SphereCast(origin, .5f, transform.forward * navMeshAgent.radius, out var raycastHit)) {

            Debug.Log(Time.time);
            
            if(raycastHit.transform.CompareTag("Player")) {
                
                _eventArchive.InvokeOnEnemyHitPlayer();
            }
        }
    }
    
    
    internal void Dead() {
        
        _canProcess = false;
    }

    internal void LowerHealth() {

        _health--;

        if(_health <= 0) {
            
            finished = player.inFinisher;
            countered = player.inCounter;
            
            isDead = true;
        }
    }
}*/