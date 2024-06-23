using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Snake : MonoBehaviour
{
    private float speed = 10;
    private float force;
    private bool isWalking = false;
    private int poisoCount = 3;

    public GameObject poison;
    public Transform firePoint;
    public Vector2 moveDirection;
    public float horizontal, vertical;

    private Animator animator;
    private Vector3 initialScale;
    private HealthBar healthBar;

    [SerializeField] private TMP_Text level;
    [SerializeField] private TMP_Text onda;

    public static Snake instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.force = 10;
        animator = GetComponent<Animator>();
        StartCoroutine(BowFireRoutine());

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
    }

    void Update()
    {
        if (healthBar.GetHealth() <= 0)
        {
            GameObject.Destroy(gameObject);
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Você pode querer manter essa função caso mova a cobra via input direto, por exemplo:
        Move(new Vector2(horizontal, vertical));
    }

    public void Move(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;

        isWalking = (this.moveDirection.x != 0 || this.moveDirection.y != 0);
        animator.SetInteger("transition", 0);

        // Verifica se a cobra está se movendo
        if (isWalking && healthBar.GetHealth() > 0)
        {
            // Define a rotação com base na direção do movimento
            if (moveDirection.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (moveDirection.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            var move = new Vector3(moveDirection.x, moveDirection.y, 0).normalized;
            transform.position += move * speed * Time.deltaTime;
            animator.SetInteger("transition", 1);
        }
        else
        {
            // Mantém a rotação atual quando a cobra não está se movendo
            // Isso evita que a direção da cobra mude ao parar de se mover
            Vector3 currentEulerAngles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentEulerAngles.x, currentEulerAngles.y, 0);
        }
    }

    private IEnumerator BowFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Fire();
        }
    }

    void Fire()
    {
        animator.SetInteger("transition", 2);
        GameObject Poison = Instantiate(poison, firePoint.position, firePoint.rotation);
        Poison bowComponent = Poison.GetComponent<Poison>();
        bowComponent.isRight = transform.rotation.y == 0;
        animator.SetInteger("transition", 1);
    }

    public void Damage(float dmg)
    {
        healthBar.Damage(dmg);
        if (healthBar.GetHealth() <= 0)
        {
            level.text = GameController.instance.levelCounter.ToString();
            onda.text = GameController.instance.countOnda.ToString();
            ScenesController.instance.GameOver();
        }
    }

    public void IncrementForce(float value) => this.force += value;

    public void Heal(float life)
    {
        healthBar.Heal(life);
    }

    public float GetLife() => healthBar.GetHealth();
    public float GetLifeMax() => healthBar.GetMaxHealth();
    public float GetForce() => this.force;
    public float GetVelocity() => this.speed;
}
