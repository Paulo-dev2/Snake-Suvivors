using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeBoss : MonoBehaviour
{
    private float MAX_EXP; // Valor inicial para MAX_EXP
    private float startLife;
    [SerializeField] private Image lifeBar;

    public static LifeBoss instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startLife = MAX_EXP;
    }

    public float GetLife() => startLife;

    void Update()
    {
        UpdateLifeBar();
    }

    public void SetMaxLife(float life)
    {
        MAX_EXP = life;
        startLife = life;
        UpdateLifeBar(); // Atualizar barra de vida após definir nova vida máxima
    }

    public void Damage(float damage)
    {
        startLife = Mathf.Max(startLife - damage, 0); // Garantir que a vida não fique negativa
        UpdateLifeBar();
    }

    private void UpdateLifeBar()
    {
        if (lifeBar != null)
        {
            lifeBar.fillAmount = startLife / MAX_EXP;
        }
    }
}