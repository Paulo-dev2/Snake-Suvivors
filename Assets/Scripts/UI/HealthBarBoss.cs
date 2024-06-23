using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    private int level;
    private float MAX_LIFE;
    private float startLife;
    [SerializeField] private Image healthBarImage;

    public static HealthBarBoss Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int GetLevel() => level;
    public void SetHealthMax(float life)
    {
        MAX_LIFE = life;
        startLife = life;
    }

    void Update()
    {
        // Se a experi�ncia for maior ou igual � experi�ncia m�xima, passa de n�vel
        UpdateLevelBar();
    }

    public void DAMAGE(float life) => MAX_LIFE -= life; // Incrementa a experi�ncia

    private void UpdateLevelBar()
    {
        healthBarImage.fillAmount = (float)startLife / MAX_LIFE; // Atualiza a barra de n�vel com a propor��o da experi�ncia atual
    }
}

