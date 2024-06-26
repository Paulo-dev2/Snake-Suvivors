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
        // Calcula a posi��o alvo onde o inimigo deve se mover
        Vector3 targetPosition = IsSnakeWithinBounds();

        // Move o inimigo suavemente em dire��o � posi��o alvo
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
    }

   
    private Vector3 IsSnakeWithinBounds()
    {
        return new Vector3(
            Mathf.Clamp(snake.position.x, -14.86f, 16f),
            Mathf.Clamp(snake.position.y, -11.80f, 8.90f),
            transform.position.z);
    }

}
