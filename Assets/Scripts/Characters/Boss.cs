using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    private Transform snake; // Referência ao transform do jogador
    private float followSpeed = 5f; // Velocidade de seguimento do inimigo
    private float health = 150;
    public float damage = 25f;
    public float visionRadius = 10f; // Raio de visão do inimigo
    public float fireRate = 1f; // Taxa de disparo (tiros por segundo)

    public float horizontal;

    public Transform firePoint;
    public GameObject bullet;
    private Animator animator;
    private Vector3 previousPosition;

    [SerializeField] private Transform damageTextPrefab;
    public UnityEvent BossDestroyedEvent;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Snake") != null)
        {
            snake = GameObject.FindGameObjectWithTag("Snake").transform;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("Ground"), true);
        animator = GetComponent<Animator>();
        StartCoroutine(BowFireRoutine());
        previousPosition = transform.position;
    }

    private void LateUpdate()
    {

        if (snake != null)
        {
            // Calcula a direção para o jogador
            float directionToPlayer = Vector2.Distance(transform.position, snake.position);
            // Move o inimigo suavemente em direção à posição alvo
            transform.position = Vector3.MoveTowards(transform.position, snake.transform.position, followSpeed * Time.deltaTime);
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
            //SearchSnake();
        }
    }

    void CheckMovementDirection()
    {
        horizontal = Input.GetAxis("Horizontal");
        transform.eulerAngles = horizontal > 0 ? new Vector3(0, 0, 0) : new Vector3(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.visionRadius);
    }
    private void SearchSnake()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, this.visionRadius);
        if (collider != null && collider.gameObject.tag == "Snake")
        {
            Debug.Log("Acertou2");
            StartCoroutine(BowFireRoutine());
        }
        else
        {
            // Se a Snake não for encontrada, pare a rotina se estiver em execução
            StopCoroutine(BowFireRoutine());
        }
    }

    public void DestroyBoss()
    {
        Destroy(gameObject, 0.5f);
        // Chamando o evento de destruição do inimigo
        if (BossDestroyedEvent != null) BossDestroyedEvent.Invoke();
    }

    public void Damage(float dmg)
    {
        var instantiateDamage = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        var damage = instantiateDamage.GetComponent<DamageText>();
        damage.SetText(dmg);
        health -= dmg;
        animator.SetTrigger("hit");

        if (health <= 0) DestroyBoss();
    }

    private IEnumerator BowFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Espera 1/2 segundos antes de disparar novamente
            Fire();
        }
    }
    
    void Fire()
    {
        animator.SetInteger("transition", 2);
        GameObject Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullet bowComponent = Bullet.GetComponent<Bullet>();
        bowComponent.isRight = transform.rotation.y == 0;
        animator.SetInteger("transition", 1);
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
