using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {

    // public GameObject enemy;

    // [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private List<EnemyBehaviour> _enemies;

    private EventArchive _eventArchive;
    
    private float _attackTime = 5f;

    private float _distanceToCheck = 10f;

    private void Awake() {

        _eventArchive = FindAnyObjectByType<EventArchive>();

        _enemies = GetComponentsInChildren<EnemyBehaviour>().ToList();
    }

    private void OnTriggerEnter(Collider other) {
        
        if(!other.CompareTag("Player")) { return; }
        
        CommandEnemies();

        StartCoroutine(SetToAttackRoutine());
    }

    private IEnumerator SetToAttackRoutine() {
        
        while(true) {
            
            yield return new WaitForSeconds(_attackTime);
            
            SetToAttack();

            yield return null;
        }
    }

    private void SetToAttack() {
        
        var validEnemies = _enemies.Where(enemy => !enemy.isDead).ToList();

        if(validEnemies.Count == 0) { return; }

        var enemyIndex = Random.Range(0, validEnemies.Count);

        
        validEnemies[enemyIndex].isAttacking = true;
    }


    private void OnTriggerExit(Collider other) {

        if(other.CompareTag("Player")) {

            PauseEnemies();
        }
    }

    private void PauseEnemies() {
        
        foreach(var enemy in _enemies) {
            
            enemy.canAttack = false;
        }
        //todo: make enemy status change to idle
    }

    private void CommandEnemies() {
        //todo: make enemy status change to strafe

        foreach(var enemy in _enemies) {
            
            enemy.canAttack = true;
        }
    }

    private void RespawnEnemies() {
        
        //todo spawn at pos or (probably this ->) spawn at start enable at trigger
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        _eventArchive.OnTargetSearch += FindClosestEnemy;
    }

    private void FindClosestEnemy(Transform player) {
        
        var distance = Mathf.Infinity;
        EnemyBehaviour target = null;
        
        foreach(var enemy in _enemies) {

            if(enemy.isDead) { continue; }
            
            var distanceToEnemy = Vector3.Distance(player.position, enemy.transform.position);

            if(!(distanceToEnemy < distance && distanceToEnemy <= _distanceToCheck)) { continue; }
            
            distance = distanceToEnemy;
            target = enemy;
        }

        if(!target) { return; }
        
        _eventArchive.InvokeOnTargetAssigned(target.transform);
        _eventArchive.InvokeOnCurrentEnemyTarget(target);
    }

    // Update is called once per frame
    void Update() {
    }
}