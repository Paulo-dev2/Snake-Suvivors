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
    private float followDistance = 2f; // Distância mínima para começar a seguir a cobra
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
            // Calcula a distância entre a maçã e a cobra
            float distanceToSnake = Vector2.Distance(transform.position, snake.transform.position);

            // Move a maçã apenas se estiver dentro da distância mínima
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
