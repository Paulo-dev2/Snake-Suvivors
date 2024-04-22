using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform enemyInGame;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
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


    private Transform SpawnEnemy() => Instantiate(enemyPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 7), 0), Quaternion.identity);
}
