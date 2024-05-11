using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private float speed = 10;
    private float horizontal = 0;
    private float vertical = 0;
    private bool isWalking = false;

    public GameObject poison;
    public Transform firePoint;

    private Animator animator;
    private Vector3 initialScale;
    private HealthBar healthBar;


    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BowFireRoutine());
        
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
    }

    void Update()
    {

        if(healthBar.GetHealth() <= 0)
        {
            GameObject.Destroy(gameObject);
        } 


        Move();
    }

    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isWalking = (horizontal != 0 || vertical != 0);
        animator.SetInteger("transition", 0);

        // Verifica se a cobra está se movendo
        if (isWalking && healthBar.GetHealth() > 0)
        {
            // Define a rotação com base na direção do movimento
            transform.eulerAngles = horizontal < 0 ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);

            var move = new Vector3(horizontal, vertical, 0).normalized;
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
            ScenesController.instance.GameOver();
        }
    }

    public void Heal(float life)
    {
        healthBar.Heal(life);
    }

    public float GetLife() => healthBar.GetHealth();
    public float GetForce() => 10f;
    public float GetVelocity() => this.speed;
}
