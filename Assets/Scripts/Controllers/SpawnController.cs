using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private Transform[] enemyPrefab;
    [SerializeField] private Transform[] boosPrefab;
    [SerializeField] private Transform applePrefab;
    [SerializeField] private Transform expPrefab;
    [SerializeField] private Transform expBigPrefab;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform magnetPrefab;
    private Transform boosInGame;

    private int initialEnemies = 2;
    private int endEnemies = 5;
    private float enemyBaseSpawnInterval = 4f;

    public static SpawnController instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnApples());
    }

    public IEnumerator SpawnApples()
    {
        while (true)
        {
            SpawnApple();
            yield return new WaitForSeconds(Random.Range(10, 20));
        }
    }

    public IEnumerator SpawnCoins()
    {
        while (true)
        {
            int numCoins = Random.Range(1, 2);
            for (int i = 0; i < numCoins; i++)
            {
                SpawnCoin();
            }
            yield return new WaitForSeconds(5f);
        }
    }

    public IEnumerator SpawnEnemies()
    {
        while (true)
        {
            int numEnemies = Random.Range(initialEnemies, endEnemies);
            for (int i = 0; i < numEnemies; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(enemyBaseSpawnInterval);
        }
    }

    public void SpawnBoss()
     {
        // Escolhe aleatoriamente um prefab de boss do array
        Transform bossPrefab = boosPrefab[UnityEngine.Random.Range(0, boosPrefab.Length)];

        // Instancia o boss e se inscreve para o evento de destruição
        boosInGame = Instantiate(bossPrefab, GetRandomSpawnPosition(8), Quaternion.identity);
        IBoss bossScript = boosInGame.GetComponent<IBoss>();
        if (bossScript != null)
            bossScript.BossDestroyedEvent.AddListener(OnBossDestroyed);
    }

    public void SpawnEnemy()
    {
        Transform enemyPrefab = this.enemyPrefab[Random.Range(0, this.enemyPrefab.Length)];
        Transform enemyInGame = Instantiate(enemyPrefab, GetRandomSpawnPosition(7), Quaternion.identity);
        enemyInGame.GetComponent<Enemy>().EnemyDestroyedEvent.AddListener(OnEnemyDestroyed);
    }

    public void SpawnApple()
    {
        Transform appleInGame = Instantiate(applePrefab, GetRandomSpawnPosition(5), Quaternion.identity);
    }

    public void SpawnMagnet()
    {
        Transform magnetInGame = Instantiate(magnetPrefab, GetRandomSpawnPosition(10), Quaternion.identity);
    }

    public void SpawnExp()
    {
        Transform expInGame = Instantiate(expPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        Exp expScript = expInGame.GetComponent<Exp>();
        int countExp = expScript.qtdExp;
        expScript.ExpDestroyedEvent.AddListener(() => GameController.instance.CheckExpEat(countExp));
    }

    public void SpawnExpBig()
    {
        Transform expInGame = Instantiate(expBigPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        Exp expScript = expInGame.GetComponent<Exp>();
        int countExp = expScript.qtdExp;
        expScript.ExpDestroyedEvent.AddListener(() => GameController.instance.CheckExpEat(countExp));
    }

    public void SpawnCoin()
    {
        Transform coinInGame = Instantiate(coinPrefab, GetRandomSpawnPosition(5), Quaternion.identity);
        Coin coinScript = coinInGame.GetComponent<Coin>();
        int countCoin = coinScript.qtdCoin;
        coinScript.CoinDestroyedEvent.AddListener(() => GameController.instance.CheckCoinEat(countCoin));
    }

    public Vector3 GetRandomSpawnPosition(float radius)
    {
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        return new Vector3(cameraPosition.x + radius, cameraPosition.y + radius, 0f);
    }

    private void OnEnemyDestroyed()
    {
        SpawnExp();
        GameController.instance.IncrementKills();
    }

    private void OnBossDestroyed()
    {
        for (int i = 0; i < 5; i++) 
            SpawnExpBig();
        for (int i = 0; i < 10; i++)
            SpawnExp();
        GameController.instance.setBossPresent();
    }

    public void IncreaseDifficulty(float k = 0.01f)
    {
        enemyBaseSpawnInterval = enemyBaseSpawnInterval * Mathf.Exp(-k);
        Enemy.instance.IncrementDamage(12f);
        Enemy.instance.IncrementHealth(16f);
    }

    public void SetEnemyRange(int initial, int end)
    {
        initialEnemies = initial;
        endEnemies = end;
    }
}
