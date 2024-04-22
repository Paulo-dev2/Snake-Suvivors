using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform snake; // Referência ao transform do jogador
    private float followSpeed = 3f; // Velocidade de seguimento do inimigo
    private float smooth = 3f;

    private Animator animator;
    private Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").transform;
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (snake != null)
        {
            // Calcula a direção para o jogador
            Vector3 directionToPlayer = snake.position - transform.position;
            // Normaliza a direção para manter a mesma velocidade em todas as distâncias
            directionToPlayer.Normalize();

            // Calcula a posição alvo onde o inimigo deve se mover
            Vector3 targetPosition = transform.position + directionToPlayer * followSpeed * Time.deltaTime;
            // Move o inimigo suavemente em direção à posição alvo
            transform.position = Vector3.Lerp(targetPosition, targetPosition, smooth * Time.deltaTime);

            // Verifica se o inimigo está se movendo
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            if (distanceMoved > 0.01f)
            {
                // O inimigo está se movendo
                animator.SetInteger("transition", 1);
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

    // Update is called once per frame
}
