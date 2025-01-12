using DG.Tweening;
using Enemy;
using UnityEngine;

public class AttackState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {
        
        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = 3f;
    }

    public void UpdateState(EnemyBehaviour enemy) {
        
        enemy.LookAtPlayer();
        
        if(enemy.isDead) {
            
            enemy.TransitionToState(new DieState());
            return;
        }

        

        if(enemy.CloseToPlayer(out var player)) {

            if(player.inAttack && enemy.isTarget) {
                
                enemy.navMeshAgent.speed = 0f;
                enemy.navMeshAgent.isStopped = true;

                if(enemy.isHit) {
                    
                    enemy.TransitionToState(new HitState());
                }
                
                return;
            }
            
            enemy.navMeshAgent.speed = 0f;
            enemy.navMeshAgent.isStopped = true;
            enemy.animator.SetTrigger("Attacking");

            enemy.StartAttack();
                
            
            if(enemy.animator.GetNextAnimatorStateInfo(0).normalizedTime >= 1f) {

                enemy.TransitionToState(new StrafeState());
                return;
            }
        }

        enemy.navMeshAgent.SetDestination(enemy.player.transform.position - (Vector3.forward + Vector3.right) * .65f);
    }
    
    public void ExitState(EnemyBehaviour enemy) {

        enemy.EndAttack();

        enemy.isAttacking = false;
        
        enemy.animator.ResetTrigger("Attacking");
    }
}