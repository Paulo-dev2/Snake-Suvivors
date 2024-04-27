using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    private Transform snake; // Refer�ncia ao transform do jogador
    private float followSpeed = 2.5f; // Velocidade de seguimento do inimigo
    private float smooth = 3f;
    private float health = 150;
    public float damage = 25f;

    private Animator animator;
    private Vector3 previousPosition;

    public UnityEvent EnemyDestroyedEvent;


    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").transform;
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (snake != null)
        {
            // Calcula a dire��o para o jogador
            Vector3 directionToPlayer = snake.position - transform.position;
            // Normaliza a dire��o para manter a mesma velocidade em todas as dist�ncias
            directionToPlayer.Normalize();

            // Calcula a posi��o alvo onde o inimigo deve se mover
            Vector3 targetPosition = transform.position + directionToPlayer * followSpeed * Time.deltaTime;
            // Move o inimigo suavemente em dire��o � posi��o alvo
            transform.position = Vector3.Lerp(targetPosition, targetPosition, smooth * Time.deltaTime);

            // Verifica se o inimigo est� se movendo
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            if (distanceMoved > 0.01f)
            {
                // O inimigo est� se movendo
                animator.SetInteger("transition", 1);
                CheckMovementDirection();
            }
            else
            {
                // O inimigo n�o est� se movendo
                animator.SetInteger("transition", 0);
            }

            // Atualiza a posi��o anterior
            previousPosition = transform.position;
        }
    }

    void CheckMovementDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.eulerAngles = horizontal > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
    }

    public void DestroyBoss()
    {
        Destroy(gameObject);
        // Chamando o evento de destrui��o do inimigo
        if (EnemyDestroyedEvent != null) EnemyDestroyedEvent.Invoke();
    }

    public void Damage(float dmg)
    {
        health -= dmg;
        animator.SetTrigger("hit");

        if (health <= 0) DestroyBoss();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Snake
        if (collision.gameObject.tag == "Snake")
        {
            Debug.Log("Atacou");
            collision.gameObject.GetComponent<Snake>().Damage(damage);
        }
    }
}
