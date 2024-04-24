using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public float scrollSpeed = 1.0f; // Velocidade de rolagem do fundo
    public Renderer backgroundRenderer; // Referência para o Renderer do fundo
    private Vector2 startPosition; // Posição inicial do fundo

    void Start()
    {
        // Salva a posição inicial do fundo
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcula o deslocamento baseado no tempo
        float offsetX = Time.time * scrollSpeed;
        float offsetY = Time.time * scrollSpeed;

        // Define a nova posição do fundo com base no deslocamento
        Vector2 offsetVector = new Vector2(offsetX, offsetY);
        backgroundRenderer.material.mainTextureOffset = offsetVector;

        // Se o fundo tiver se movido completamente para a esquerda ou para baixo,
        // resete sua posição
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
