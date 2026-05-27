// Estado de perseguição do inimigo
// Responsável por fazer o inimigo seguir o player
public class EnemyChaseState : EnemyState
{
    // Construtor do estado
    // Recebe:
    // - o EnemyController
    // - a StateMachine
    public EnemyChaseState(
        EnemyController enemy,
        EnemyStateMachine stateMachine
    ) : base(enemy, stateMachine)
    {
    }

    // Executado quando o inimigo entra no estado de perseguição
    public override void Enter()
    {
        // Libera o NavMeshAgent para se mover
        enemy.agent.isStopped = false;

        // Ativa a animação de caminhada
        enemy.anim.SetBool("isWalk", true);
    }

    // Executado a cada frame enquanto estiver perseguindo
    public override void Update()
    {
        // Se não existir player, volta para Idle
        if (enemy.player == null)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        // Define o destino do inimigo como a posição do player
        enemy.agent.SetDestination(enemy.player.position);

        // Se chegou perto o suficiente, troca para ataque
        if (enemy.DistanceToPlayer() <= enemy.attackDistance)
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        // Se o player saiu do raio de perseguição,
        // o inimigo entra em alerta antes de desistir
        if (enemy.DistanceToPlayer() > enemy.chaseDistance)
        {
            stateMachine.ChangeState(enemy.alertState);
        }
    }

    // Executado quando sai do estado de perseguição
    public override void Exit()
    {
        // Desativa a animação de caminhada
        enemy.anim.SetBool("isWalk", false);
    }
}