using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour {
    
    private EventArchive _eventArchive;


    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
    }
    
    void Start() {
        
    }

    void Update() {
    }
}