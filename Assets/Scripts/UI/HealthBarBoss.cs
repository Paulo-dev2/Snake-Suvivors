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
        // Se a experiência for maior ou igual à experiência máxima, passa de nível
        UpdateLevelBar();
    }

    public void DAMAGE(float life) => MAX_LIFE -= life; // Incrementa a experiência

    private void UpdateLevelBar()
    {
        healthBarImage.fillAmount = (float)startLife / MAX_LIFE; // Atualiza a barra de nível com a proporção da experiência atual
    }
}

