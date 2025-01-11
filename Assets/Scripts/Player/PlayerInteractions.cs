using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInteractions : MonoBehaviour {
    
    private EventArchive _eventArchive;

    private CharacterController _charCon;

    private Vector2 _input;

    private Camera _mainCam;
    
    
    //todo: subscribe to player specific events and make the character move
    
    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
        
        _charCon = GetComponentInChildren<CharacterController>();
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

        Debug.DrawRay(origin, transform.forward + Vector3.forward * .5f, Color.blue);
        
        if(Physics.SphereCast(origin, .5f, transform.forward, out var raycastHit)) {

            if(raycastHit.transform.CompareTag("Enemy")) {
                
                Debug.Log($"hit enemy! {raycastHit.transform.name}");
            }
        }
    }
    

    void Update() {
        
        
        
    }
}