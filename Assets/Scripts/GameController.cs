using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text timeCurrent;
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform boosPrefab;
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
        if (time >= 10 && !bossSpawned)
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
            SpawnBos();
            yield return new WaitForSeconds(40);
        }
    }

    private void SpawnBos()
    {
        boosInGame = Instantiate(boosPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 7), 0), Quaternion.identity);
        //boosInGame.GetComponent<Enemy>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);
    }

    private void SpawnEnemy()
    {
        // Instancia o inimigo e se inscreve para o evento de destruição
        enemyInGame = Instantiate(enemyPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 7), 0), Quaternion.identity);
        enemyInGame.GetComponent<Enemy>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);
    }

    private void SpawnApple() => appleInGame = Instantiate(applePrefab, new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 7), 0), Quaternion.identity);

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
