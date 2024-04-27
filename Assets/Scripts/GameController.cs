using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text level;
    [SerializeField] private TMP_Text timeCurrent;
    [SerializeField] private Transform[] enemyPrefab;
    [SerializeField] private Transform[] boosPrefab;
    [SerializeField] private Transform applePrefab;
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject gameOverObj;

    private bool isPaused;
    private Transform appleInGame;
    private Transform enemyInGame;
    private Transform boosInGame;
    private float time;
    private bool bossSpawned = false;


    public static GameController instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        SpawnApple();
        time = Time.time;
    }

    private void Update()
    {
        time += Time.deltaTime;
        timeCurrent.text = time.ToString();
        NormalizeTimeDisplay();
        CheckAppleStatus();
        CheckTimeStatus();
        PauseGame();
    }

    private void NormalizeTimeDisplay()
    {
        // Crie um TimeSpan com o tempo em segundos
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        // Formate o tempo em mm:ss
        string timeFormatted = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

        // Atualize o texto no TextMeshPro
        timeCurrent.text = timeFormatted;
    }

    private void CheckTimeStatus()
    {
        if (time >= 30 && !bossSpawned)
        {
            StartCoroutine(SpawnBoss());
            bossSpawned = true;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Gera um número aleatório entre 1 e 3 para determinar quantos inimigos aparecerão
            int numEnemies = UnityEngine.Random.Range(1, 4);

            // Loop para criar o número especificado de inimigos
            for (int i = 0; i < numEnemies; i++)
            {
                SpawnEnemy();
            }

            // Aguarda 1 segundo antes de continuar para o próximo spawn
            yield return new WaitForSeconds(3.5f);
        }
    }

    private IEnumerator SpawnBoss()
    {
        while (true)
        {
            // Espera até que o boss não tenha sido spawnado antes de instanciá-lo novamente
            if (!bossSpawned)
            {
                // Escolhe aleatoriamente um prefab de boss do array
                Transform bossPrefab = boosPrefab[UnityEngine.Random.Range(0, boosPrefab.Length)];

                // Instancia o boss e se inscreve para o evento de destruição
                boosInGame = Instantiate(bossPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                boosInGame.GetComponent<Boss>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);

                bossSpawned = true; // Define o flag para true para indicar que um boss foi spawnado

                // Aguarda um tempo aleatório antes de tentar instanciar o próximo boss
                yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 45));

                bossSpawned = false; // Define o flag para false para indicar que um novo boss pode ser spawnado
            }

            yield return null;
        }
    }


    private void SpawnEnemy()
    {
        // Escolhe aleatoriamente um prefab de boss do array
        Transform iniPrefab = enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)];

        // Instancia o boss e se inscreve para o evento de destruição
        Transform enemyInGame = Instantiate(iniPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        // Instancia o inimigo e se inscreve para o evento de destruição
        enemyInGame.GetComponent<Enemy>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);
    }

    private void SpawnApple() => appleInGame = Instantiate(applePrefab, GetRandomSpawnPosition(), Quaternion.identity);

    private Vector3 GetRandomSpawnPosition() => new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-7f, 7f), 0f);

    private void CheckAppleStatus()
    {
        if (appleInGame == null)
        {
            SpawnApple();
        }
    }

    // Método chamado quando um inimigo é destruído
    void OnEnemyDestroyed() =>  SpawnApple();

    public void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            isPaused = !isPaused;
            pauseObj.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void GameOver()
    {
        gameOverObj.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame() => SceneManager.LoadScene(1);
}
