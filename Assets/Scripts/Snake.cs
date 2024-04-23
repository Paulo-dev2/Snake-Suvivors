using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private float speed = 6;
    private float horizontal = 0;
    private float vertical = 0;
    private bool isWalking = false;
    public float health;

    public GameObject bow;
    public Transform firePoint;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(BowFireRoutine());
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Se não pressionar nada o valor é 0, se pressionar para direita vai até o 1 e se pressionar esquerda vai até -1
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isWalking = (horizontal != 0 || vertical != 0);
        animator.SetInteger("transition", 0);
        transform.eulerAngles = horizontal < 0 ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
        if (isWalking)
        {
            var move = new Vector3(horizontal, vertical, 0).normalized;
            transform.position += move * speed * Time.deltaTime;
            animator.SetInteger("transition", 1);
        }
    }

    private IEnumerator BowFireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f); // Espera 3 segundos antes de disparar novamente
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
        yield return new WaitForSeconds(0.5f); // Espera 1 segundo antes de resetar a animação
        animator.SetInteger("transition", 0);
    }
}
