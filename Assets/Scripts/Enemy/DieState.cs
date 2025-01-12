namespace Enemy {
    public class DieState : IEnemyState {
    
        public void EnterState(EnemyBehaviour enemy) {
            enemy.navMeshAgent.isStopped = true;
            enemy.navMeshAgent.speed = 0;

            enemy.animator.SetTrigger(enemy.finished ? "DeadAlt" : "Dead");
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
}