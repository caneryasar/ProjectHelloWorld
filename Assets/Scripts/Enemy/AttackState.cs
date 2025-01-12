using DG.Tweening;
using UnityEngine;

public class AttackState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {
        
        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = 3f;
    }

    public void UpdateState(EnemyBehaviour enemy) {
        
        if(enemy.isDead) {
            
            enemy.TransitionToState(new DieState());
            return;
        }

        if(enemy.isHit) {
            
            enemy.TransitionToState(new HitState());
            return;
        }

        if(enemy.CloseToPlayer()) {

            enemy.navMeshAgent.speed = 0f;
            enemy.navMeshAgent.isStopped = true;
            enemy.animator.SetTrigger("Attacking");
            
            DOVirtual.DelayedCall(1.5f, () => enemy.TransitionToState(new StrafeState()));
            return;
        }

        enemy.navMeshAgent.SetDestination(enemy.player.transform.position - (Vector3.forward + Vector3.right) * .65f);
        enemy.LookAtPlayer();
    }
    
    public void ExitState(EnemyBehaviour enemy) {

        enemy.isAttacking = false;
        
        enemy.animator.ResetTrigger("Attacking");
    }
}