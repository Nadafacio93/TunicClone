using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator anim;

    [Header("Config")]
    public float chaseDistance = 8f;
    public float attackDistance = 2f;
    public float attackInterval = 1.2f;

    [Header("Controle")]
    public bool isDead = false;
    public bool isTakingHit = false;

    public EnemyStateMachine stateMachine;

    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyAlertState alertState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        alertState = new EnemyAlertState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        if (isDead || isTakingHit) return;

        stateMachine.Update();
    }

    public float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;

        return Vector3.Distance(transform.position, player.position);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        isTakingHit = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (anim != null)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isDead", true);
        }

        Destroy(gameObject, 3f);
    }

    public void BeginHitReaction()
    {
        if (isDead) return;

        isTakingHit = true;

        if (agent != null && agent.enabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (anim != null)
        {
            anim.SetBool("isWalk", false);
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Hit");
        }
    }

    public void EndHitReaction()
    {
        if (isDead) return;

        isTakingHit = false;
        EvaluateStateByDistance();
    }

    public void EvaluateStateByDistance()
    {
        if (isDead || stateMachine == null || stateMachine.currentState == null)
        {
            return;
        }

        EnemyState nextState;
        float distanceToPlayer = DistanceToPlayer();

        if (distanceToPlayer <= attackDistance)
        {
            nextState = attackState;
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            nextState = chaseState;
        }
        else
        {
            // Quando o player esta longe, entra em alert antes de desistir
            // completamente da perseguicao e voltar para idle.
            nextState = alertState;
        }

        // Forca re-enter para religar corretamente animacao/movimento
        // depois que um hit interrompeu o estado anterior.
        stateMachine.ChangeState(nextState, true);
    }


}
