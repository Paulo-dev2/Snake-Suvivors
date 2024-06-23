using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
public class Enemy : MonoBehaviour
{

    private Transform snake; // Referência ao transform do jogador
    public float followSpeed = 5f; // Velocidade de seguimento do inimigo
    private float health = 20;
    private float damage = 8f;

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
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Ground"), true);
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
            // Calcula a direção para o jogador
            float directionToPlayer = Vector2.Distance(transform.position, snake.position);
            // Move o inimigo suavemente em direção à posição alvo
            transform.position = Vector3.MoveTowards(transform.position, snake.transform.position, followSpeed *  Time.deltaTime);

            if (directionToPlayer > 0.01f)
            {
                // O inimigo está se movendo
                animator.SetInteger("transition", 1);
                CheckMovementDirection();
            }
            else
            {
                // O inimigo não está se movendo
                animator.SetInteger("transition", 0);
            }

            // Atualiza a posição anterior
            previousPosition = transform.position;
        }
    }

    public static void DestroyAllEnemies()
    {
        // Encontre todos os objetos inimigos na cena e destrua-os
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.DestroyEnemy();
        }
    }

     void CheckMovementDirection()
    {
        Vector3 directionToSnake = snake.position - transform.position;
        if (directionToSnake.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Enfrentando a direita
        }
        else if (directionToSnake.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Enfrentando a esquerda
        }
    }

    public void DestroyEnemy()
    {
        animator.SetTrigger("hit");
        Destroy(gameObject, 0.5f);
        // Chamando o evento de destruição do inimigo
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

    public void IncrementDamage(float force) => this.damage += force;
    public void IncrementHealth(float life) => this.health += life;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Snake
        if (collision.gameObject.tag == "Snake")
        {
            collision.gameObject.GetComponent<Snake>().Damage(damage);
        }
    }
}
