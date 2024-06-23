using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Golem : MonoBehaviour, IBoss
{
    private Transform snake; // Referência ao transform do jogador
    private float followSpeed = 5f; // Velocidade de seguimento do inimigo
    private float health;
    public float damage;
    public float visionRadius = 10f; // Raio de visão do inimigo
    public float fireRate = 1f; // Taxa de disparo (tiros por segundo)

    public float horizontal;

    public Transform firePoint;
    public GameObject bullet;
    private Animator animator;
    private Vector3 previousPosition;
    private Transform instantiateHealth;
    private HealthBarBoss objHealth;

    [SerializeField] private Transform damageTextPrefab;
    [SerializeField] private RectTransform healthPrefab;
    [SerializeField] private Transform healthPoint;
    public UnityEvent BossDestroyedEvent { get; private set; }

    private Coroutine bowFireRoutine;

    private void Awake()
    {
        BossDestroyedEvent = new UnityEvent();
    }

    void Start()
    {
        GameObject snakeObject = GameObject.FindGameObjectWithTag("Snake");
        //instantiateHealth = Instantiate(healthPrefab, healthPoint.position, healthPoint.rotation);
        //instantiateHealth.SetParent(healthPoint);
        //objHealth = instantiateHealth.GetComponent<HealthBarBoss>();
        if (snakeObject != null)
        {
            snake = snakeObject.transform;
            SetHealth();
            SetDamage();
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("Ground"), true);
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    private void SetDamage()
    {
        this.damage = (Snake.instance.GetLifeMax() * 0.08f);
    }

    private void SetHealth()
    {
        if (Snake.instance != null)
        {
            var life = Snake.instance.GetLifeMax() * 3;
            this.health = life;
        }
        else
        {
            Debug.LogError("Snake instance is not set.");
        }
    }

    private void LateUpdate()
    {
        if (snake != null)
        {
            float directionToPlayer = Vector2.Distance(transform.position, snake.position);
            transform.position = Vector3.MoveTowards(transform.position, snake.transform.position, followSpeed * Time.deltaTime);
            if (directionToPlayer > 0.01f)
            {
                animator.SetInteger("transition", 1);
                CheckMovementDirection();
            }
            else
            {
                animator.SetInteger("transition", 0);
            }
            previousPosition = transform.position;
            SearchSnake(snake.position);
        }
    }

    public void CheckMovementDirection()
    {
        Vector3 directionToSnake = snake.position - transform.position;
        if (directionToSnake.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Enfrentando a direita
        }
        else if (directionToSnake.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Enfrentando a esquerda
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.visionRadius);
    }

    private void SearchSnake(Vector2 positionSnake)
    {
        Vector3 newVector = new Vector3(positionSnake.x + visionRadius, positionSnake.y + visionRadius, 0);
        if (Vector2.Distance(this.transform.position, newVector) >= visionRadius)
        {
            if (bowFireRoutine == null)
            {
                bowFireRoutine = StartCoroutine(BowFireRoutine());
            }
        }
        else
        {
            if (bowFireRoutine != null)
            {
                StopCoroutine(bowFireRoutine);
                bowFireRoutine = null;
            }
        }
    }

    public void DestroyBoss()
    {
        Destroy(gameObject, 0.5f);
        if (BossDestroyedEvent != null) BossDestroyedEvent.Invoke();
    }

    public void Damage(float dmg)
    {
        var instantiateDamage = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        var damage = instantiateDamage.GetComponent<DamageText>();
        damage.SetText(dmg);
        health -= dmg;
        LifeBoss.instance.Damage(dmg);
        Debug.Log(health);
        //objHealth.DAMAGE(dmg);
        animator.SetTrigger("hit");

        if (health <= 0) DestroyBoss();
    }

    private IEnumerator BowFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate);
            Fire();
        }
    }

    public void Fire()
    {
        animator.SetInteger("transition", 2);
        GameObject Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullet bowComponent = Bullet.GetComponent<Bullet>();
        bowComponent.isRight = transform.eulerAngles.y == 180;
        animator.SetInteger("transition", 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Snake")
        {
            collision.gameObject.GetComponent<Snake>().Damage(damage);
        }
    }
}
