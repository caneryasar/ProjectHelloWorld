using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

    private EventArchive _eventArchive;

    private CapsuleCollider _collider;
    
    
    //todo: subscribe to player specific events and make the character move
    
    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
    }

    void Start() {
        
    }

    void Update() {
        
    }
}