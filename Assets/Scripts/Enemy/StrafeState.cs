using UnityEngine;

public class StrafeState : IEnemyState {
    private Vector3 targetPosition;

    public void EnterState(EnemyBehaviour enemy) {
        
        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = 2;
        enemy.animator.SetTrigger("Strafing");
        targetPosition = enemy.GenerateTargetPosition();
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

        if(enemy.isAttacking) {
            
            enemy.TransitionToState(new AttackState());
            return;
        }

        if(targetPosition == Vector3.zero || enemy.ArrivedAtPosition(targetPosition)) {
            
            targetPosition = enemy.GenerateTargetPosition();
            return;
        }

        enemy.UpdateAnimatorDirection(targetPosition);
        enemy.navMeshAgent.SetDestination(targetPosition);
        enemy.LookAtPlayer();
    }

    public void ExitState(EnemyBehaviour enemy) {
        
        enemy.animator.ResetTrigger("Strafing");
        enemy.animator.SetFloat("DirectionZ", 0);
        enemy.animator.SetFloat("DirectionX", 0);
    }
}