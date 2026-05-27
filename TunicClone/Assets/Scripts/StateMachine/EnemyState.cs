// Classe base de todos os estados do inimigo
// "abstract" significa que ela serve apenas como modelo
// e não pode ser instanciada diretamente
public abstract class EnemyState
{
    // Referência do controlador do inimigo
    // protected = acessível apenas pelas classes filhas
    // readonly = valor definido apenas no construtor
    protected readonly EnemyController enemy;

    // Referência da máquina de estados
    protected readonly EnemyStateMachine stateMachine;

    // Construtor da classe base
    // Todo estado recebe:
    // - o inimigo
    // - a máquina de estados
    protected EnemyState(
        EnemyController enemy,
        EnemyStateMachine stateMachine)
    {
        // Guarda referência do inimigo
        this.enemy = enemy;

        // Guarda referência da state machine
        this.stateMachine = stateMachine;
    }

    // Método executado ao entrar no estado
    // virtual = pode ser sobrescrito nas classes filhas
    public virtual void Enter() { }

    // Método executado todo frame
    public virtual void Update() { }

    // Método executado ao sair do estado
    public virtual void Exit() { }
}