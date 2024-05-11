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

        // Move o inimigo suavemente em direção à posição alvo
        if(snake != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(snake.position.x, snake.position.y, -10), smooth * Time.deltaTime);
        } else
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }

}
