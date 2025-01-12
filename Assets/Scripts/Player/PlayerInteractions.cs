
using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class PlayerInteractions : MonoBehaviour {
    
    // public AudioResource walkSFX;
    // public AudioResource runSFX;
    public AudioResource jumpSFX;
    public AudioResource doublejumpSFX;
    public AudioResource hitSFX;
    public AudioResource attackSFX;
    
    private AudioSource _audioSource;
    
    private EventArchive _eventArchive;

    private CharacterController _charCon;

    private CinemachineImpulseSource _impulseSource;

    private Vector2 _input;

    private Camera _mainCam;

    private bool _targetSearch;
    
    
    
    //todo: subscribe to player specific events and make the character move
    
    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
        _audioSource = FindAnyObjectByType<AudioSource>();
        
        _charCon = GetComponentInChildren<CharacterController>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();
        
        
    }

    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Enemy")) {

            var enemy = other.GetComponent<EnemyBehaviour>();

            if(enemy.isAttacking) {
                
                Debug.Log($"enemy is counterable: {enemy.name}");
                
                _eventArchive.InvokeOnCounterable(enemy);
                
                return;
            }
        }
    }

    void Start() {

        _eventArchive.OnFocusHold += FindTarget;
    }

    private void FindTarget(bool focused) {

        if(!focused) {
            
            _eventArchive.InvokeOnResetCamTarget();
        }
    }

    public void CheckForEnemyHit() {

        var origin = transform.position + (Vector3.up * _charCon.height * .5f);
        
        if(Physics.SphereCast(origin, .5f, transform.forward, out var raycastHit, 1f)) {
            
            Debug.Log($"check hit: {raycastHit.transform.name}");
            
            if(raycastHit.transform.CompareTag("Enemy")) {
                
                if(raycastHit.transform.GetComponent<EnemyBehaviour>().isDead) { return; }
                
                _eventArchive.InvokeOnPlayerHitEnemy(raycastHit.transform.GetComponent<EnemyBehaviour>());
                
                _impulseSource.GenerateImpulse();
            }
        }
    }
    

    void Update() {
    }

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3.up * _charCon.height * .5f) + transform.forward, .5f);


        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(transform.position + (Vector3.up * _charCon.height * .5f), 3f);
    }
}