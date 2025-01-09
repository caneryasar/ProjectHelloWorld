using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private EventArchive _eventArchive;


    private void Awake() {

        _eventArchive = FindAnyObjectByType<EventArchive>();
    }

    void Start() {
        
        _eventArchive.InvokeOnPlayable(true);
    }
    
    void Update() {
        
    }
}