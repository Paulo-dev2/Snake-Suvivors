using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
public class Enemy : MonoBehaviour
{

    private Transform snake; // Refer�ncia ao transform do jogador
    public float followSpeed = 5f; // Velocidade de seguimento do inimigo
    public float health = 20;
    public float damage = 8f;

    private Animator animator;
    private Vector3 previousPosition;

    public UnityEvent EnemyDestroyedEvent;
    [SerializeField] private Transform damageTextPrefab;

    public static Enemy instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Snake") != null)
        {
            snake = GameObject.FindGameObjectWithTag("Snake").transform;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Ground"), true);
        animator = GetComponent<Animator>();
        previousPosition = transform.position;

    }

    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (snake != null)
        {
            // Calcula a dire��o para o jogador
            float directionToPlayer = Vector2.Distance(transform.position, snake.position);
            // Move o inimigo suavemente em dire��o � posi��o alvo
            transform.position = Vector3.MoveTowards(transform.position, snake.transform.position, followSpeed *  Time.deltaTime);

            if (directionToPlayer > 0.01f)
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

    public void DestroyEnemy()
    {
        animator.SetTrigger("hit");
        Destroy(gameObject, 0.5f);
        // Chamando o evento de destrui��o do inimigo
        if (EnemyDestroyedEvent != null) EnemyDestroyedEvent.Invoke();
    }

    public void Damage(float dmg)
    {
        var instantiateDamage = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        var damage = instantiateDamage.GetComponent<DamageText>();
        damage.SetText(dmg);
        health -= dmg;

        if (health <= 0) DestroyEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Snake
        if (collision.gameObject.tag == "Snake")
        {
            collision.gameObject.GetComponent<Snake>().Damage(damage);
        }
    }
}