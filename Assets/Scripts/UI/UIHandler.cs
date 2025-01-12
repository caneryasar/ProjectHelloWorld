using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    public GameObject infoPanel;

    public TextMeshProUGUI targetName;
    public Image healthBar;

    private EventArchive _eventArchive;

    private EnemyBehaviour _currentEnemy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake() {
        _eventArchive = FindFirstObjectByType<EventArchive>();
    }

    void Start() {
        
        _eventArchive.OnResetCamTarget += () => _currentEnemy = null;
        _eventArchive.OnCurrentEnemyTarget += enemy => {
            _currentEnemy = enemy;
        };
        _eventArchive.OnFocusHold += focus => {

            if(!focus || !_currentEnemy) {
                
                infoPanel.SetActive(false);
                return;
            }
            
            infoPanel.SetActive(focus);
            targetName.text = _currentEnemy.name;
            healthBar.fillAmount = _currentEnemy.health / 3f;
        };
    }

    // Update is called once per frame
    void Update() {
    }
}