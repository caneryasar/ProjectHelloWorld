public class AttackState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {
        
        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = 7.5f;
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
            
            enemy.navMeshAgent.isStopped = true;
            enemy.animator.SetTrigger("Attacking");
            enemy.TransitionToState(new StrafeState());
            return;
        }

        enemy.navMeshAgent.SetDestination(enemy.player.transform.position);
        enemy.LookAtPlayer();
    }
    
    public void ExitState(EnemyBehaviour enemy) {
        
        enemy.animator.ResetTrigger("Attacking");
    }
}