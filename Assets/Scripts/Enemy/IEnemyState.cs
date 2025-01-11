public interface IEnemyState {
    
    void EnterState(EnemyBehaviour enemy);
    
    void UpdateState(EnemyBehaviour enemy);
    
    void ExitState(EnemyBehaviour enemy);
}