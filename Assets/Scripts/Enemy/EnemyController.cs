using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    // public GameObject enemy;

    // [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private List<EnemyBehaviour> _enemies;

    private EventArchive _eventArchive;

    private int _activeEnemies;

    private bool _playerInSight;

    private void Awake() {

        _eventArchive = FindAnyObjectByType<EventArchive>();

        _enemies = GetComponentsInChildren<EnemyBehaviour>().ToList();
        // _enemies = FindObjectsByType<Enemy>(0).ToList();
        _activeEnemies = _enemies.Count;
    }

    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player")) {

            _playerInSight = true;
            
            CommandEnemies();
        }
    }

    private void OnTriggerExit(Collider other) {

        if(other.CompareTag("Player")) {

            _playerInSight = false;

            PauseEnemies();
        }
    }

    private void PauseEnemies() {
        
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

            if(!(distanceToEnemy < distance)) { continue; }
            
            distance = distanceToEnemy;
            target = enemy;
        }
        
        if(target) { _eventArchive.InvokeOnTargetAssigned(target.transform); }
    }

    // Update is called once per frame
    void Update() {
    }
}