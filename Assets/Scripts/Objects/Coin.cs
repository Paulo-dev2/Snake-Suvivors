using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent CoinDestroyedEvent;
    public Snake snake;
    private Animator animator;
    public int qtdCoin;
    private float velocity = 10f;
    private float followDistance = 2f; // Dist�ncia m�nima para come�ar a seguir a cobra
    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").GetComponent<Snake>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetInteger("transition", 0);
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
            if (CoinDestroyedEvent != null) CoinDestroyedEvent.Invoke();
        }
    }
}
