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

    [Header("Cameras")]
    [SerializeField] private GameObject playerCamera;

    [Header("Particulas")]
    [SerializeField] private ParticleSystem fxAtack;
    private bool isAttacking;

    [Header("Attack")]
    public Transform hitBox;
    [Range(0.2f, 1f)]//Barra Deslizante
    public float hitRange = 0.5f;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Collider[] hitInfo;
    public int amountDmg = 25;//Aqui

    [Header("Dano")]
    public int hp = 100;
    private bool isDie = false;

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
        //Pega  os eixos X e Z
        Vector3 direction =
            new Vector3(moveInput.x, 0, moveInput.y);


        if (direction.magnitude > 0.1f)
        {

            isWalk = true;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;//Ele Calcula o angulo baseado na direção
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);//Suaviza a rotação

            //Personagem Rotacione baseado na movimentação
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f);
        }
        else
        {
            isWalk = false;
        }


        controller.Move(
            direction *
            speedMovement *
            Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        anim.SetBool("isWalk", isWalk);

    }

    private void AtaquePlayer()
    {
        if (!isAttacking)
        {
            anim.SetTrigger("triggerAtack");
            fxAtack.Emit(1);
            isAttacking = true;

            hitInfo = Physics.OverlapSphere
                (hitBox.position, hitRange, hitLayer);

            foreach (Collider hit in hitInfo)
            {
                
                hit.gameObject.SendMessage
                    ("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "CamTrigger":
                playerCamera.SetActive(true);
                break;
        }
        if (other.gameObject.CompareTag("TakeDamage"))
        {
            GetHit(10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                playerCamera.SetActive(false);
                break;
        }
    }

    public void AtackIsDone()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hitBox.position, hitRange);
    }

    public void GetHit(int amount)
    {
        if (isDie) return;
        hp = hp - amount;

        if (hp > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            anim.SetTrigger("Die");
            isDie = true;
        }
    }
}
