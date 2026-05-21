using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Config Player")]
    private CharacterController controller;

    [SerializeField] private float speedMovement = 3f;
    [SerializeField] private float gravity = -9.81f;

    private Vector2 moveInput;
    private Vector3 velocity;

    [Header("Animação")]
    private Animator anim;
    public bool isWalk;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnAttack()
    {
        AtaquePlayer();

    }
    private void Update()
    {
        MovimentacaoPlayer();
    }

    private void MovimentacaoPlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        if (direction.magnitude > 0.1f)
        {
            isWalk = true;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;//Calcula o angulo baseado na direção
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);//Suaviza a rotação

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);//Personagem rotaciona baseado na movimentação
        }
        else
        {
            isWalk = false;
        }

        controller.Move(direction * speedMovement * Time.deltaTime);
        if(controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        anim.SetBool("isWalk", isWalk);
    }

    private void AtaquePlayer()
    {
        anim.SetTrigger("triggerAttack");
    }
}
