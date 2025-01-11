public class IdleState : IEnemyState {
    public void EnterState(EnemyBehaviour enemy) {
        enemy.navMeshAgent.isStopped = true;
        enemy.navMeshAgent.speed = 0;
        enemy.animator.SetTrigger("Idling");
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

        if(enemy.canAttack) {
            enemy.TransitionToState(new StrafeState());
            return;
        }

        enemy.LookAtPlayer();
    }

    public void ExitState(EnemyBehaviour enemy) {
        enemy.animator.ResetTrigger("Idling");
    }
}