using UnityEngine;

public class PlayerMechanics : MonoBehaviour {

    private EventArchive _eventArchive;


    //todo: subscribe to player specific events and make the character move
    
    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
    }

    void Start() {
        
    }

    void Update() {
        
    }
}