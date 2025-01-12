using Enemy;
using UnityEngine;

public class HitState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {

        if(enemy.isTarget) { enemy.EndAttack(); }
        
        enemy.navMeshAgent.isStopped = true;
        enemy.navMeshAgent.speed = 0;
        
        if(enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) {
            
            enemy.animator.Play("Hit", -1, 0);
        }
        else {
            
            enemy.animator.SetTrigger("Hit");
        }
    }

    public void UpdateState(EnemyBehaviour enemy) {

        enemy.LookAtPlayer();
        
        if(enemy.isDead) {
            
            enemy.TransitionToState(new DieState());
            return;
        }
        
        
        if(enemy.CloseToPlayer(out var player)) {

            if(player.inAttack && enemy.isTarget) {
                
                enemy.TransitionToState(new HitState());
                return;
            }
        }
        
        if(enemy.isHit) {
            
            if(enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) {
            
                enemy.animator.Play("Hit", -1, 0);
            }
            else {
            
                enemy.animator.SetTrigger("Hit");
            }
        }

        if(enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            
            enemy.TransitionToState(new IdleState());
        }
    }

    public void ExitState(EnemyBehaviour enemy) {

        enemy.isHit = false;
        
        enemy.animator.ResetTrigger("Hit");
    }
}