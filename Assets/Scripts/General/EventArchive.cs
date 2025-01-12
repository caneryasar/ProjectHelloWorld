using System;
using Unity.VisualScripting;
using UnityEngine;

public class EventArchive : MonoBehaviour {

    
    //todo: add player region and player specific events (onplayermove etc.)

    #region Anims
    
    public event Action OnComboBroken;
    public event Action OnSecondAttack;
    public event Action OnThirdAttack;
    public event Action<int> OnAttackCountChange;
    public event Action<Vector2> OnDirectionChangeFocused; 

    public void InvokeOnComboBroken() { OnComboBroken?.Invoke(); }
    public void InvokeOnSecondAttack() { OnSecondAttack?.Invoke(); }
    public void InvokeOnThirdAttack() { OnThirdAttack?.Invoke(); }
    public void InvokeOnAttackCountChange(int count) { OnAttackCountChange?.Invoke(count);}
    public void InvokeOnDirectionChangeFocused(Vector2 direction) { OnDirectionChangeFocused?.Invoke(direction);}
    
    #endregion
    
    #region GamePlay

    //Events
    public event Action<bool> OnPlayable;
    public event Action<bool> OnDoubleJump;
    public event Action OnResetJump;
    public event Action<EnemyBehaviour> OnPlayerHitEnemy;
    public event Action OnEnemyHitPlayer;
    public event Action<Transform> OnTargetSearch; 
    public event Action<Transform> OnTargetAssigned;
    public event Action OnResetCamTarget;
    public event Action<int> OnCounterTriggered;
    public event Action<bool, EnemyBehaviour> OnCounterable;
    public event Action<EnemyBehaviour> OnCurrentEnemyTarget;
    
    //Methods
    public void InvokeOnPlayable(bool isPlayable) { OnPlayable?.Invoke(isPlayable); }
    public void InvokeOnDoubleJump(bool isJumped) { OnDoubleJump?.Invoke(isJumped); }
    public void InvokeOnResetJump() { OnResetJump?.Invoke(); }
    public void InvokeOnPlayerHitEnemy(EnemyBehaviour hit) { OnPlayerHitEnemy?.Invoke(hit); }
    public void InvokeOnEnemyHitPlayer() { OnEnemyHitPlayer?.Invoke(); }
    
    public void InvokeOnTargetAssigned(Transform target) { OnTargetAssigned?.Invoke(target);}
    public void InvokeOnTargetSearch(Transform player) { OnTargetSearch?.Invoke(player);}
    public void InvokeOnResetCamTarget() { OnResetCamTarget?.Invoke(); }
    public void InvokeOnCounterTriggered(int counter) { OnCounterTriggered?.Invoke(counter);}
    public void InvokeOnCounterable(bool canCounter, EnemyBehaviour counter) { OnCounterable?.Invoke(canCounter, counter);}
    public void InvokeOnCurrentEnemyTarget(EnemyBehaviour target) { OnCurrentEnemyTarget?.Invoke(target);}

    #endregion
    
    #region Input

    //Events
    public event Action<Vector2> OnMoveInput;
    public event Action<bool> OnSprintInput;
    public event Action OnJumpTriggered;
    public event Action OnDashTriggered;
    public event Action<bool> OnJumpInput;
    public event Action OnFocus;
    public event Action<bool> OnFocusHold;
    public event Action OnAttack;
    public event Action On2ndAttack;
    public event Action On3rdAttack; 
    
    //Methods
    public void InvokeOnMoveInput(Vector2 moveInput) { OnMoveInput?.Invoke(moveInput); }
    public void InvokeOnSprintInput(bool sprintInput) { OnSprintInput?.Invoke(sprintInput); }
    public void InvokeOnJumpInput(bool jumpInput) { OnJumpInput?.Invoke(jumpInput); }
    public void InvokeOnJumpTriggered() { OnJumpTriggered?.Invoke(); }
    public void InvokeOnDashTriggered() { OnDashTriggered?.Invoke(); }
    public void InvokeOnFocus() { OnFocus?.Invoke(); }
    public void InvokeOnFocusHold(bool isHolding) { OnFocusHold?.Invoke(isHolding); }
    public void InvokeOnAttack() { OnAttack?.Invoke(); }
    public void InvokeOn2ndAttack() { On2ndAttack?.Invoke(); }
    public void InvokeOn3rdAttack() { On3rdAttack?.Invoke(); }
        
    #endregion
    
}