using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text timeCurrent;
    [SerializeField] private TMP_Text countCoinCurrent;
    [SerializeField] private TMP_Text countKillsCurrent;
    [SerializeField] private TMP_Text ondaCurrent;
    [SerializeField] private Transform[] enemyPrefab;
    [SerializeField] private Transform[] boosPrefab;
    [SerializeField] private Transform applePrefab;
    [SerializeField] private Transform expPrefab;
    [SerializeField] private Transform expBigPrefab;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform magnetPrefab;

    private Transform appleInGame;
    private Transform enemyInGame;
    private Transform boosInGame;

    private int levelCounter = 1;
    private int countOnda = 1;
    private const float LEVEL_DURATION = 10f; // Duração de 1.5 minutos em segundos
    private float ENEMY_BASE_SPAWN_INTERVAL = 4f; // Intervalo de spawn base de inimigos

    private float time;
    private bool bossSpawned = false;
    private int countEatCoin;

    private LevelBar levelBar;
    public static GameController instance;

    private const int increment = 1;
    private int inicial_inimigos = 2;
    private int end_inimigos = 5;
    private int killsInimigos;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnCoins());
        time = LEVEL_DURATION;
        levelBar = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelBar>();
        levelCounter = 0;
        StartCoroutine(SpawnApples());
        //StartCoroutine(SpawnMagnets());
    }

    void Update()
    {
        levelCounter = levelBar.GetLevel();
        if (time > 0)
        {
            time -= Time.deltaTime;
            countCoinCurrent.text = countEatCoin.ToString();
            ondaCurrent.text = countOnda.ToString();
            countKillsCurrent.text = killsInimigos.ToString();
            NormalizeTimeDisplay();
            OnInventario();
        }
        UpdateLevelTimer();
    }

    private void UpdateLevelTimer()
    {
        // Verifica se o temporizador do nível chegou a zero 
        if (time <= 0)
        {
            inicial_inimigos += increment;
            end_inimigos += increment;
            ScenesController.instance.OndPassed();
            time = LEVEL_DURATION; // Reinicia o temporizador do nível
            countOnda++;
            IncreaseDifficulty();

            // Verifica se é hora de spawnar um boss (a cada 5 níveis)
            if (countOnda % 5 == 0 && countOnda != 0)
            {
                StartCoroutine(SpawnBoss());
                bossSpawned = true; // Define o sinalizador de bossSpawned como true
            }
        }
    }

    private void IncreaseDifficulty()
    {
        float k = 0.01f;  // constante para controlar a rapidez com que o tempo diminui
        ENEMY_BASE_SPAWN_INTERVAL = ENEMY_BASE_SPAWN_INTERVAL * (float) Math.Exp(-k);
        Enemy.instance.health += (countOnda / 100);
        Enemy.instance.damage += (countOnda / 100);
        Enemy.instance.followSpeed += (countOnda / 100);
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


    public IEnumerator SpawnApples()
    {
        while (true)
        {
            SpawnApple();
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 20));
        }
    }

    public IEnumerator SpawnMagnets()
    {
        while (true)
        {
            SpawnMagnet();
            yield return new WaitForSeconds(UnityEngine.Random.Range(20, 30));
        }
    }


    // Método chamado quando um inimigo é destruído

    public IEnumerator SpawnCoins()
    {
        while (true)
        {
            int numCoins = UnityEngine.Random.Range(1, 2);

            // Loop para criar o número especificado de inimigos
            for (int i = 0; i < numCoins; i++)
            {
                SpawnCoin();
            }

            // Aguarda 1 segundo antes de continuar para o próximo spawn
            yield return new WaitForSeconds(5f);
        }
    }

    public IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Gera um número aleatório entre 1 e 3 para determinar quantos inimigos aparecerão
            int numEnemies = UnityEngine.Random.Range(inicial_inimigos, end_inimigos);

            // Loop para criar o número especificado de inimigos
            for (int i = 0; i < numEnemies; i++)
            {
                SpawnEnemy();
            }

            // Aguarda 1 segundo antes de continuar para o próximo spawn
            yield return new WaitForSeconds(ENEMY_BASE_SPAWN_INTERVAL);
        }
    }

    public IEnumerator SpawnBoss()
    {
        while (true)
        {
            // Espera até que o boss não tenha sido spawnado antes de instanciá-lo novamente
            if (!bossSpawned)
            {
                // Escolhe aleatoriamente um prefab de boss do array
                Transform bossPrefab = boosPrefab[UnityEngine.Random.Range(0, boosPrefab.Length)];

                // Instancia o boss e se inscreve para o evento de destruição
                boosInGame = Instantiate(bossPrefab, GetRandomSpawnPosition(8), Quaternion.identity);
                boosInGame.GetComponent<Boss>().BossDestroyedEvent.AddListener(OnBossDestroyed);

                bossSpawned = true; // Define o flag para true para indicar que um boss foi spawnado

                // Aguarda um tempo aleatório antes de tentar instanciar o próximo boss
                yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 45));

                bossSpawned = false; // Define o flag para false para indicar que um novo boss pode ser spawnado
            }

            yield return null;
        }
    }


    public void SpawnEnemy()
    {
        // Escolhe aleatoriamente um prefab de boss do array
        Transform iniPrefab = enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)];

        // Instancia o boss e se inscreve para o evento de destruição
        Transform enemyInGame = Instantiate(iniPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        // Instancia o inimigo e se inscreve para o evento de destruição
        enemyInGame.GetComponent<Enemy>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);
    }

    public void CheckExpEat(int numberCountsExp) => levelBar.EXP(numberCountsExp);

    public void CheckCoinEat(int countCoin) => countEatCoin += countCoin;


    public void SpawnApple()
    {
        appleInGame = Instantiate(applePrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        appleInGame.GetComponent<Apple>();
    }

    public void SpawnMagnet()
    {
        var magnetInGame = Instantiate(magnetPrefab, GetRandomSpawnPosition(2), Quaternion.identity);
        magnetInGame.GetComponent<Magnet>().Teste();
    }

    public void SpawnExp()
    {
        var expInGame = Instantiate(expPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        var instantiateExp = expInGame.GetComponent<Exp>();
        var countExp = instantiateExp.qtdExp;
        instantiateExp.ExpDestroyedEvent.AddListener(() => CheckExpEat(countExp));
    }

    public void SpawnExpBig()
    {
        var expInGame = Instantiate(expBigPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        var instantiateExp = expInGame.GetComponent<Exp>();
        var countExp = instantiateExp.qtdExp;
        instantiateExp.ExpDestroyedEvent.AddListener(() => CheckExpEat(countExp));
    }

    public void SpawnCoin()
    {
        var coinInGame = Instantiate(coinPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        var instantiateCoin = coinInGame.GetComponent<Coin>();
        var countCoin = instantiateCoin.qtdCoin;
        instantiateCoin.CoinDestroyedEvent.AddListener(() => CheckCoinEat(countCoin));
    }

    public Vector3 GetRandomSpawnPosition(float radius)
    {
        // Obtém a posição atual da cobra
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        // Retorna a posição dentro dos limites da área de jogo
        return new Vector3(cameraPosition.x + radius, cameraPosition.y + radius, 0f);

    }

    // Método chamado quando um inimigo é destruído
    public void OnEnemyDestroyed()
    {
        SpawnExp();
        killsInimigos++;
    }

    public void OnBossDestroyed() => SpawnExpBig();

    public void OnInventario(){
        if(Input.GetKeyDown(KeyCode.E))
            ScenesController.instance.Iventario();
    }
}
