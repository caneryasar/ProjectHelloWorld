using Enemy;

public class IdleState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {
        enemy.navMeshAgent.isStopped = true;
        enemy.navMeshAgent.speed = 0;
        enemy.animator.SetTrigger("Idling");
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

        if(enemy.canAttack) {
            enemy.TransitionToState(new StrafeState());
            return;
        }
    }

    public void ExitState(EnemyBehaviour enemy) {
        enemy.animator.ResetTrigger("Idling");
    }
}