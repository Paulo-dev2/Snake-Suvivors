using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public float scrollSpeed = 1.0f; // Velocidade de rolagem do fundo
    public Renderer backgroundRenderer; // Refer�ncia para o Renderer do fundo
    private Vector2 startPosition; // Posi��o inicial do fundo

    void Start()
    {
        // Salva a posi��o inicial do fundo
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcula o deslocamento baseado no tempo
        float offsetX = Time.time * scrollSpeed;
        float offsetY = Time.time * scrollSpeed;

        // Define a nova posi��o do fundo com base no deslocamento
        Vector2 offsetVector = new Vector2(offsetX, offsetY);
        backgroundRenderer.material.mainTextureOffset = offsetVector;

        // Se o fundo tiver se movido completamente para a esquerda ou para baixo,
        // resete sua posi��o
        if (transform.position.x < startPosition.x - backgroundRenderer.bounds.size.x)
        {
            transform.position = new Vector2(startPosition.x, transform.position.y);
        }
        if (transform.position.y < startPosition.y - backgroundRenderer.bounds.size.y)
        {
            transform.position = new Vector2(transform.position.x, startPosition.y);
        }
    }
}
