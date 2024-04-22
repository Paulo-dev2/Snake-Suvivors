using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Transform snake;
    public float smooth;
    // Start is called before the first frame update
    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // Calcula a posição alvo onde o inimigo deve se mover
        Vector3 targetPosition = IsSnakeWithinBounds();

        // Move o inimigo suavemente em direção à posição alvo
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
    }

   
    private Vector3 IsSnakeWithinBounds()
    {
        return new Vector3(
            Mathf.Clamp(snake.position.x, -13.11f, 13.8f),
            Mathf.Clamp(snake.position.y, -9.9f, 7f),
            transform.position.z);
    }

}
