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
            Exp.instance.SetFollowDistance(magnetRadius);
            Destroy(gameObject);
        }
    }
}
