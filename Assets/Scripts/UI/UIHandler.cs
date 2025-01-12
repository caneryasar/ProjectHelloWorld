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
    void Start() {
        
        _eventArchive = FindFirstObjectByType<EventArchive>();
        _eventArchive.OnFocusHold += focus => {

            if(!focus || !_currentEnemy) {
                
                infoPanel.SetActive(false);
                return;
            }
            
            infoPanel.SetActive(focus);
        };
        _eventArchive.OnCurrentEnemyTarget += enemy => {
            
            _currentEnemy = enemy;
            targetName.text = _currentEnemy.name;
            healthBar.fillAmount = _currentEnemy.health / 3f;
        };
        _eventArchive.OnResetCamTarget += () => _currentEnemy = null;
    }

    // Update is called once per frame
    void Update() {
    }
}