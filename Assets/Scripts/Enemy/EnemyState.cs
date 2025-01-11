using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState {
    
    public enum STATE { IDLE, STRAFE, HIT, ATTACK, DIE };

    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE state;
    protected EVENT stage;
    protected Enemy enemy;
    protected GameObject model;
    protected Transform playerTransform;
    protected Animator animator;
    protected NavMeshAgent agent;
    // protected PlayerController player;
    protected EnemyState nextEnemyState;

    
    //Params
    private readonly float _punchDistance = 1f;
    protected float strafeDistance = 5f;

    public EnemyState(Enemy m_Enemy) {

        enemy = m_Enemy;
        model = m_Enemy.gameObject;
        // player = m_Enemy.player;
        // playerTransform = player.transform;
        playerTransform = m_Enemy.player.transform;
        animator = m_Enemy.animator;
        agent = m_Enemy.navMeshAgent;
        
        
        stage = EVENT.ENTER;
    }

    protected virtual void Enter() { stage = EVENT.UPDATE; }

    protected virtual void Update() { stage = EVENT.UPDATE; }

    protected virtual void Exit() { stage = EVENT.EXIT; }

    public EnemyState Process() {
        
        if(stage == EVENT.ENTER) Enter();
        if(stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT) {
            
            Exit();
            return nextEnemyState;
        }

        return this;
    }
    
    public bool CloseToPunch() {

        var distance = (playerTransform.position - model.transform.position).magnitude;

        return (distance < _punchDistance);
    }

    public Vector3 GenerateTargetPosition() {

        var randomPoint = playerTransform.position + Random.onUnitSphere * Random.Range(strafeDistance * .5f, strafeDistance * 1.5f);

        return NavMesh.SamplePosition(randomPoint, out var hit, 3f, NavMesh.AllAreas) ? hit.position : Vector3.zero;
    }

    public bool ArrivedToPosition(Vector3 target) {

        var distance = Vector3.Distance(target, model.transform.position);

        return distance < .5f;
    }

    public Vector2 CalculateDirection(Vector3 target) {

        var worldDirection = (target - model.transform.position).normalized;

        var localDirection = model.transform.InverseTransformDirection(worldDirection);

        var calculatedDirection = Vector2.up * localDirection.z + Vector2.right * localDirection.x;
        
        return calculatedDirection;
    }
    
}

public class Idle : EnemyState {
    
    public Idle(Enemy m_Enemy) : base(m_Enemy) {

        agent.speed = 0;
        agent.isStopped = true;
        
        state = STATE.IDLE;
    }

    protected override void Enter() {
        
                
        animator.SetTrigger("Idling");
        
        base.Enter();
    }

    protected override void Update() {
        
        
        if(enemy.isDead) {
            
            nextEnemyState = new Die(enemy);
            stage = EVENT.EXIT;
        }

        if(enemy.isHit) {
            
            nextEnemyState = new Hit(enemy);
            stage = EVENT.EXIT;
        }
        
        model.transform.LookAt(new Vector3(playerTransform.position.x, model.transform.position.y, playerTransform.position.z));
        
        if(!enemy.canAttack) { return; }
        
        nextEnemyState = new Strafe(enemy);
        
        stage = EVENT.EXIT;
    }

    protected override void Exit() {
        
                
        animator.ResetTrigger("Idling");
        base.Exit();
    }
}
public class Strafe : EnemyState {

    private Vector3 _target;
    
    public Strafe(Enemy m_Enemy) : base(m_Enemy) {
  
        agent.speed = 2;
        agent.isStopped = false;
        
        state = STATE.STRAFE;
    }

    protected override void Enter() {
        
                
        animator.SetTrigger("Strafing");

        _target = GenerateTargetPosition();
        
        base.Enter();
    }

    protected override void Update() {

        if(_target == Vector3.zero) {
            
            _target = GenerateTargetPosition();
            
            return;
        }

        if(enemy.isDead) {
            
            nextEnemyState = new Die(enemy);
            stage = EVENT.EXIT;
        }

        if(enemy.isHit) {
            
            nextEnemyState = new Hit(enemy);
            stage = EVENT.EXIT;
        }
        
        // model.transform.LookAt(new Vector3(playerTransform.position.x, model.transform.position.y, playerTransform.position.z));

        if(enemy.isAttacking) {

            nextEnemyState = new Attack(enemy);
            stage = EVENT.EXIT;
        }
        
        var direction = CalculateDirection(_target);

        Debug.Log(direction);
        
        animator.SetFloat("DirectionZ", direction.y);
        animator.SetFloat("DirectionX", direction.x);
        
        if(ArrivedToPosition(_target)) {

            _target = GenerateTargetPosition();
        }
        
        agent.SetDestination(_target);
        model.transform.LookAt(playerTransform.position);
        
        Debug.Log($"distance: {Vector3.Distance(agent.transform.position, _target)}");
    }

    protected override void Exit() {        
        
        
        animator.ResetTrigger("Strafing");

        animator.SetFloat("DirectionZ", 0);
        animator.SetFloat("DirectionX", 0);
        base.Exit();
    }
}
public class Hit : EnemyState {
    
    public Hit(Enemy m_Enemy) : base(m_Enemy) {
        
        agent.speed = 0;
        agent.isStopped = true;
        
        state = STATE.HIT;
    }

    protected override void Enter() {        
        
        
        animator.SetTrigger("Hit");
        enemy.LowerHealth();
        base.Enter();
    }

    protected override void Update() {        
        

        if(enemy.isDead) {
            
            nextEnemyState = new Die(enemy);
            stage = EVENT.EXIT;
        }
        

        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .8f) {

            nextEnemyState = new Idle(enemy);
            stage = EVENT.EXIT;
        }
    }

    protected override void Exit() {        
        
        
        
        animator.ResetTrigger("Hit");
        base.Exit();
    }
}
public class Attack : EnemyState {
    
    public Attack(Enemy _enemy) : base(_enemy) {
        
        agent.speed = 7.5f;
        agent.isStopped = false;
        
        
        state = STATE.ATTACK;
    }

    protected override void Enter() {        
        
        
        base.Enter();
    }

    protected override void Update() { 
        
        
        if(enemy.isDead) {
            
            nextEnemyState = new Die(enemy);
            stage = EVENT.EXIT;
        }

        if(enemy.isHit) {
            
            nextEnemyState = new Hit(enemy);
            stage = EVENT.EXIT;
        }
        
        
        if(CloseToPunch()) {

            agent.isStopped = true;
            
            animator.SetTrigger("Attacking");

            nextEnemyState = new Strafe(enemy);
            
            stage = EVENT.EXIT;
        }
        
        
        agent.SetDestination(playerTransform.position);
        model.transform.LookAt(new Vector3(playerTransform.position.x, model.transform.position.y, playerTransform.position.z));

    }

    protected override void Exit() { 
        
                
        animator.ResetTrigger("Attacking");
        base.Exit();
    }
}
public class Die : EnemyState {

    private bool _normalDeath;
    
    public Die(Enemy _enemy) : base(_enemy) {

        _normalDeath = enemy.normalDeath;
        
        state = STATE.DIE;
    }

    protected override void Enter() {
        
        animator.SetTrigger(_normalDeath ? "Dead" : "DeadAlt");

        enemy.Dead();
        
                base.Exit();
    }

    protected override void Update() {
        
        
        //stage = EVENT.EXIT;
    }

    protected override void Exit() {
        
        animator.ResetTrigger("Dead");
        animator.ResetTrigger("DeadAlt");
        
        base.Exit();
    }
}
