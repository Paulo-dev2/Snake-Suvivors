using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text timeCurrent;
    [SerializeField] private TMP_Text countCoinCurrent;
    [SerializeField] private TMP_Text countKillsCurrent;
    [SerializeField] private TMP_Text ondaCurrent;

    [SerializeField] private GameObject LevelBarObj;
    [SerializeField] private GameObject LifeBarObj;

    public int levelCounter;
    public int countOnda = 1;
    private const float LEVEL_DURATION = 60f;
    private float time;
    private int countEatCoin;

    private LevelBar levelBar;
    public static GameController instance;

    private const int increment = 1;
    private int initialEnemies = 2;
    private int endEnemies = 5;
    private int killsInimigos;

    private bool isBossPresent = false; // Variável para verificar se o chefe está presente

    void Awake()
    {
        instance = this;
        LevelBarObj.SetActive(true);
        LifeBarObj.SetActive(false);
    }

    void Start()
    {
        time = LEVEL_DURATION;
        levelBar = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelBar>();
        levelCounter = 0;
    }

    void Update()
    {
        if (!isBossPresent) // Só decrementar o tempo se o chefe não estiver presente
        {
            LifeBarObj.SetActive(false);
            LevelBarObj.SetActive(true);
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                UpdateLevelTimer();
            }
        }
        else
        {
            LevelBarObj.SetActive(false);
        }

        countCoinCurrent.text = countEatCoin.ToString();
        ondaCurrent.text = countOnda.ToString();
        countKillsCurrent.text = killsInimigos.ToString();
        NormalizeTimeDisplay();
    }

    public void setBossPresent()
    {
        isBossPresent = false;
        countOnda++;
    }

    private void UpdateLevelTimer()
    {
        if (time <= 0)
        {
            if (countOnda % 3 == 0 && countOnda != 0)
                ScenesController.instance.OndPassed();

            initialEnemies += increment;
            endEnemies += increment;
            time = LEVEL_DURATION;
            countOnda++;
            SpawnController.instance.SetEnemyRange(initialEnemies, endEnemies);
            SpawnController.instance.IncreaseDifficulty();

            if (countOnda % 5 == 0 && countOnda != 0)
            {
                var life = Snake.instance.GetLifeMax() * 3;
                LifeBarObj.SetActive(true);
                isBossPresent = true; // Chefe está presente
                LifeBoss.instance.SetMaxLife(life);
                SpawnController.instance.SpawnBoss();
            }
            else
            {
                Enemy.DestroyAllEnemies();
            }
        }
    }

    private void NormalizeTimeDisplay()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string timeFormatted = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        timeCurrent.text = timeFormatted;
    }

    public void CheckExpEat(int numberCountsExp) => levelBar.EXP(numberCountsExp);

    public void CheckCoinEat(int countCoin) => countEatCoin += countCoin;

    public void IncrementKills() => killsInimigos++;
}
