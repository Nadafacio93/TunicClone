public class EnemyStateMachine
{
    // Estado atual do inimigo
    public EnemyState currentState;

    // Inicia a máquina de estados com um estado inicial
    public void Initialize(EnemyState startState)
    {
        // Se não recebeu estado inicial, não faz nada
        if (startState == null)
        {
            return;
        }

        // Define o estado atual
        currentState = startState;

        // Chama o método Enter do estado inicial
        currentState.Enter();
    }

    // Troca o estado atual por outro estado
    public void ChangeState(EnemyState newState, bool forceReenter = false)
    {
        // Se o novo estado for nulo, ignora
        if (newState == null)
        {
            return;
        }

        // Se tentar entrar no mesmo estado, ignora
        // A não ser que forceReenter seja true
        if (!forceReenter && newState == currentState)
        {
            return;
        }

        // Sai do estado atual, se existir
        currentState?.Exit();

        // Define o novo estado
        currentState = newState;

        // Entra no novo estado
        currentState.Enter();
    }

    // Atualiza o estado atual
    public void Update()
    {
        // Chama o Update do estado atual, se existir
        currentState?.Update();
    }
}