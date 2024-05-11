using UnityEngine;
using UnityEngine.Events;

public class Exp : MonoBehaviour
{
    public UnityEvent ExpDestroyedEvent;
    public Snake snake;
    public int qtdExp;

    private float velocity = 10f;
    private float followDistance = 2f; // Dist�ncia m�nima para come�ar a seguir a cobra

    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").GetComponent<Snake>();
    }

    private void Update()
    {
        FollowSnake();
    }

    private void FollowSnake()
    {
        if (snake != null)
        {
            // Calcula a dist�ncia entre a ma�� e a cobra
            float distanceToSnake = Vector2.Distance(transform.position, snake.transform.position);

            // Move a ma�� apenas se estiver dentro da dist�ncia m�nima
            if (distanceToSnake <= followDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, snake.transform.position, velocity * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Snake")
        {
            Destroy(gameObject);
            if (ExpDestroyedEvent != null) ExpDestroyedEvent.Invoke();
        }
    }
}
