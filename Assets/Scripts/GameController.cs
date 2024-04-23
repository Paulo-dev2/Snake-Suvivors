using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] public Transform applePrefab;
    private Transform appleInGame;
    private Transform enemyInGame;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        SpawnApple();
    }

    private void Update()
    {
        CheckAppleStatus();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Gera um número aleatório entre 1 e 3 para determinar quantos inimigos aparecerão
            int numEnemies = Random.Range(1, 4);

            // Loop para criar o número especificado de inimigos
            for (int i = 0; i < numEnemies; i++)
            {
                SpawnEnemy();
            }

            // Aguarda 1 segundo antes de continuar para o próximo spawn
            yield return new WaitForSeconds(3.5f);
        }
    }

    private void SpawnEnemy()
    {
        // Instancia o inimigo e se inscreve para o evento de destruição
        enemyInGame = Instantiate(enemyPrefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 7), 0), Quaternion.identity);
        enemyInGame.GetComponent<Enemy>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);
    }

    private void SpawnApple()
    {
        appleInGame = Instantiate(applePrefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 7), 0), Quaternion.identity);
    }

    private void CheckAppleStatus()
    {
        if (appleInGame == null)
        {
            SpawnApple();
        }
    }

    // Método chamado quando um inimigo é destruído
    void OnEnemyDestroyed()
    {
        // Quando um inimigo é destruído, chamamos a função SpawnApple() para substituí-lo por uma maçã
        SpawnApple();
    }
}
