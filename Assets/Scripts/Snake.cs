using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private float health = 300;
    private float speed = 6;
    private float horizontal = 0;
    private float vertical = 0;
    private bool isWalking = false;

    public GameObject bow;
    public Transform firePoint;
    public Transform objectHealth;

    private Animator animator;
    private Vector3 initialScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BowFireRoutine());

        initialScale = objectHealth.localScale;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isWalking = (horizontal != 0 || vertical != 0);
        animator.SetInteger("transition", 0);

        // Verifica se a cobra est� se movendo
        if (isWalking)
        {
            // Define a rota��o com base na dire��o do movimento
            transform.eulerAngles = horizontal < 0 ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);

            var move = new Vector3(horizontal, vertical, 0).normalized;
            transform.position += move * speed * Time.deltaTime;
            animator.SetInteger("transition", 1);
        }
        else
        {
            // Mant�m a rota��o atual quando a cobra n�o est� se movendo
            // Isso evita que a dire��o da cobra mude ao parar de se mover
            Vector3 currentEulerAngles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentEulerAngles.x, currentEulerAngles.y, 0);
        }
    }

    private IEnumerator BowFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Espera 1 segundos antes de disparar novamente
            Fire();
        }
    }

    void Fire()
    {
        animator.SetInteger("transition", 2);
        GameObject Bow = Instantiate(bow, firePoint.position, firePoint.rotation);
        Bow bowComponent = Bow.GetComponent<Bow>();
        bowComponent.isRight = transform.rotation.y == 0;
        StartCoroutine(ResetAnimation());
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Espera 1 segundo antes de resetar a anima��o
        animator.SetInteger("transition", 0);
    }

    public void Damage(float dmg)
    {
        health -= dmg;

        // Atualiza a escala do objeto de sa�de conforme a quantidade de dano recebida
        float newScaleX = Mathf.Clamp(health / 100f, 0f, 1f);
        objectHealth.localScale = new Vector3(initialScale.x * newScaleX, initialScale.y, initialScale.z);

        if (health <= 0)
        {
            GameController.instance.GameOver();
        }
    }
}
