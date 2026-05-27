using UnityEngine;

// Estado de alerta do inimigo
// O inimigo para por alguns segundos e observa o jogador
public class EnemyAlertState : EnemyState
{
    // Temporizador interno do estado
    private float timer;

    // Tempo que o inimigo ficará observando
    private const float alertDuration = 2f;

    // Construtor do estado
    // Recebe:
    // - o inimigo
    // - a máquina de estados
    public EnemyAlertState(
        EnemyController enemy,
        EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
    }

    // Executado ao entrar no estado
    public override void Enter()
    {
        // Reinicia o timer
        timer = 0f;

        // Para o NavMeshAgent
        enemy.agent.isStopped = true;

        // Desativa animação de caminhada
        enemy.anim.SetBool("isWalk", false);

        // Ativa animação de observação
        enemy.anim.SetBool("isLook", true);
    }

    // Executado todo frame
    public override void Update()
    {
        // Soma o tempo passado desde o último frame
        timer += Time.deltaTime;

        // Verifica se existe player
        if (enemy.player != null)
        {
            // Faz o inimigo olhar para o player
            enemy.transform.LookAt(enemy.player);
        }

        // Enquanto o tempo não acabar,
        // permanece no estado de alerta
        if (timer < alertDuration)
        {
            return;
        }

        // Após alguns segundos,
        // decide o próximo estado

        // Se o player estiver perto:
        // volta a perseguir
        if (enemy.DistanceToPlayer() <= enemy.chaseDistance)
        {
            stateMachine.ChangeState(enemy.chaseState);
        }
        else
        {
            // Se o player estiver longe:
            // volta para idle
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    // Executado ao sair do estado
    public override void Exit()
    {
        // Desativa animação de observação
        enemy.anim.SetBool("isLook", false);
    }
}