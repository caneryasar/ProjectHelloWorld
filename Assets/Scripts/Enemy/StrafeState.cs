using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;
public class StrafeState : IEnemyState
{
    private float _angle;              
    private float _radius = 8f;        
    private float _speed = 50f;        
    private Vector3 _targetPosition;
    private float _directionFactor;


    public void EnterState(EnemyBehaviour enemy)
    {
        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = 1;
        enemy.animator.SetTrigger("Strafing");

        // Initialize the angle for this enemy
        _angle = Random.Range(0f, 360f);

        _radius = Random.Range(_radius - _radius * .75f, _radius + _radius * .75f);

        _directionFactor = Random.value < .5f ? -1 : 1;
        
        // Calculate the initial target position
        _targetPosition = enemy.CalculateSphericalTargetPosition(_angle, _radius);
    }

    public void UpdateState(EnemyBehaviour enemy)
    {
        if (enemy.isDead)
        {
            enemy.TransitionToState(new DieState());
            return;
        }

        if(enemy.CloseToPlayer(out var player)) {

            if(player.inAttack && enemy.isTarget) {
                
                enemy.TransitionToState(new HitState());
                return;
            }
        }
        
        /*if (enemy.isHit)
        {
            enemy.TransitionToState(new HitState());
            return;
        }*/

        if (enemy.isAttacking)
        {
            enemy.TransitionToState(new AttackState());
            return;
        }

        // Update the angle for rotation
        _angle += _speed * Time.deltaTime * _directionFactor; // Clockwise rotation
        // if (_angle >= 360f) _angle -= 360f;

        _angle %= 360f;

        // Calculate the new target position
        _targetPosition = enemy.CalculateSphericalTargetPosition(_angle, _radius);

        // Set the target position for the NavMeshAgent
        enemy.navMeshAgent.SetDestination(_targetPosition);

        // Update animator for directional movement
        enemy.UpdateAnimatorDirection(_targetPosition);

        // Orient the enemy toward the player
        enemy.LookAtPlayer();
    }

    public void ExitState(EnemyBehaviour enemy)
    {
        enemy.animator.ResetTrigger("Strafing");
        enemy.animator.SetFloat("DirectionZ", 0);
        enemy.animator.SetFloat("DirectionX", 0);
    }
}

/*
public class StrafeState : IEnemyState {
    
    private Vector3 _targetPosition;
    private Vector3 _moveDirection;

    public void EnterState(EnemyBehaviour enemy) {
        
        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = 2;
        enemy.animator.SetTrigger("Strafing");
        _targetPosition = enemy.GenerateTargetPosition();
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

        /*
        if(targetPosition == Vector3.zero || enemy.ArrivedAtPosition(targetPosition)) {
            
            targetPosition = enemy.GenerateTargetPosition();
            return;
        }#1#

        if(_targetPosition == Vector3.zero || enemy.ArrivedAtPosition(_targetPosition)) {

            if(Random.Range(0, 2) > 0) {

                _moveDirection = enemy.transform.right;
                
                return;
            }

            _moveDirection = -enemy.transform.right;
            
            return;
        }

        enemy.navMeshAgent.Move(_moveDirection);

        enemy.UpdateAnimatorDirection(_targetPosition);
        enemy.navMeshAgent.SetDestination(_targetPosition);
        enemy.LookAtPlayer();
    }

    public void ExitState(EnemyBehaviour enemy) {
        
        enemy.animator.ResetTrigger("Strafing");
        enemy.animator.SetFloat("DirectionZ", 0);
        enemy.animator.SetFloat("DirectionX", 0);
    }
}*/