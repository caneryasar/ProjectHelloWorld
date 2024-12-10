using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private EventArchive _eventArchive;


    private void Awake() {
        
        _eventArchive = FindAnyObjectByType<EventArchive>();
    }

    void Start() {
        
    }

    void Update() {
        
    }
}