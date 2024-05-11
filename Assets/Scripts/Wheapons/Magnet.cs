using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float magnetStrength = 5f; // Força do magnetismo
    public float magnetRadius = 10f; // Raio de alcance do magnetismo
    public LayerMask magnetizableLayer; // Camada dos objetos que podem ser magnetizados
    public Snake snake;

    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").GetComponent<Snake>();
    }


    public void Teste()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius, magnetizableLayer);
        Debug.Log(colliders);

        foreach (Collider2D collider in colliders)
        {
            // Verifica se o objeto possui o componente Rigidbody2D
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calcula a direção para onde puxar o objeto
                Vector2 direction = (transform.position - collider.transform.position).normalized;

                // Aplica a força de magnetismo
                rb.velocity = Vector2.MoveTowards(rb.velocity, direction * magnetStrength, Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha uma esfera gizmo para visualizar a área de alcance do magnetismo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Snake")
        {
            Destroy(gameObject);
        }
    }
    
}
