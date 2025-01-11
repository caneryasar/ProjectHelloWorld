public class DieState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {
        enemy.navMeshAgent.isStopped = true;
        enemy.navMeshAgent.speed = 0;
        
        if(enemy.finished) {
            
            enemy.animator.SetTrigger("DeadAlt");
        }
        else if(enemy.countered) {
            
            enemy.animator.SetTrigger("Dead");
        }
        else {
            
            enemy.animator.SetTrigger("Dead");
        }
    }

    public void UpdateState(EnemyBehaviour enemy) {
        // Once dead, no further updates are required
        enemy.Dead();
    }

    public void ExitState(EnemyBehaviour enemy) {
        enemy.animator.ResetTrigger("Dead");
        enemy.animator.ResetTrigger("DeadAlt");
    }
}