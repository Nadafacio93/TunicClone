// Estado Idle do inimigo
// Estado padrão onde o inimigo fica parado
public class EnemyIdleState : EnemyState
{
    // Construtor do estado
    // Recebe:
    // - o EnemyController
    // - a StateMachine
    public EnemyIdleState(
        EnemyController enemy,
        EnemyStateMachine stateMachine
    ) : base(enemy, stateMachine)
    {
    }

    // Executado ao entrar no estado Idle
    public override void Enter()
    {
        // Faz o NavMeshAgent parar
        enemy.agent.isStopped = true;

        // Garante que a animação de caminhada esteja desligada
        enemy.anim.SetBool("isWalk", false);
    }

    // Executado todo frame enquanto estiver no Idle
    public override void Update()
    {
        // Verifica se o player entrou no raio de perseguição
        if (enemy.DistanceToPlayer() <= enemy.chaseDistance)
        {
            // Ao detectar o player,
            // o inimigo entra primeiro em alerta
            // antes de começar a perseguir
            stateMachine.ChangeState(enemy.alertState);
        }
    }
}