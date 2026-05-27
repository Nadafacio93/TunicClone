// Estado de ataque do inimigo
// Responsável por atacar o player enquanto ele estiver perto
public class EnemyAttackState : EnemyState
{
    // Controla o tempo entre ataques
    private float attackTimer;

    // Construtor do estado
    // Recebe:
    // - o inimigo
    // - a máquina de estados
    public EnemyAttackState(
        EnemyController enemy,
        EnemyStateMachine stateMachine
    ) : base(enemy, stateMachine)
    {
    }

    // Executado ao entrar no estado
    public override void Enter()
    {
        // Reinicia o temporizador de ataque
        attackTimer = 0f;

        // Faz o inimigo parar de andar
        enemy.agent.isStopped = true;

        // Executa animação de ataque
        enemy.anim.SetTrigger("Attack");
    }

    // Executado todo frame
    public override void Update()
    {
        // Se o player estiver longe,
        // volta para perseguição
        if (enemy.DistanceToPlayer() > enemy.attackDistance)
        {
            stateMachine.ChangeState(enemy.chaseState);
            return;
        }

        // Soma o tempo desde o último frame
        attackTimer += UnityEngine.Time.deltaTime;

        // Verifica se já pode atacar novamente
        if (attackTimer >= enemy.attackInterval)
        {
            // Reinicia o timer
            attackTimer = 0f;

            // Executa novo ataque
            enemy.anim.SetTrigger("Attack");
        }
    }
}