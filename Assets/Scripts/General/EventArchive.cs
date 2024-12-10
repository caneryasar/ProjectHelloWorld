using System;
using Unity.VisualScripting;
using UnityEngine;

public class EventArchive : MonoBehaviour {

    
    //todo: add player region and player specific events (onplayermove etc.)
    
    #region Events

    #region Input
    
    public event Action<Vector2> OnMoveInput;
    public event Action<Vector2> OnLookInput;
    public event Action<bool> OnSprintInput;
    public event Action<bool> OnJumpInput;
    public event Action<bool> OnAttackInput;
    public event Action<bool> OnShootInput; 

    #endregion

    #endregion


    #region Methods

    #region Input
    
    public void InvokeOnMoveInput(Vector2 moveInput) { OnMoveInput?.Invoke(moveInput); }
    public void InvokeOnLookInput(Vector2 lookInput) { OnLookInput?.Invoke(lookInput); }
    public void InvokeOnSprintInput(bool sprintInput) { OnSprintInput?.Invoke(sprintInput); }
    public void InvokeOnJumpInput(bool jumpInput) { OnJumpInput?.Invoke(jumpInput); }
    public void InvokeOnAttackInput(bool attackInput) { OnAttackInput?.Invoke(attackInput); }
    public void InvokeOnShootInput(bool isShooting) { OnShootInput?.Invoke(isShooting); }

    #endregion

    #endregion
}