using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    private int level;
    private int MAX_EXP;
    private int startExp = 0;
    [SerializeField] private Image levelBar;
    [SerializeField] private TMP_Text Level;

    void Start()
    {
        MAX_EXP = 150; // Defina um valor inicial para MAX_EXP
    }

    public int GetLevel() => level;

    void Update()
    {
        // Se a experiência for maior ou igual à experiência máxima, passa de nível
        UpdateLevelBar();
        if (startExp >= MAX_EXP)
        {
            level++; // Incrementa o nível
            startExp = 0; // Remove a experiência necessária para passar de nível
            Level.text = level.ToString(); // Atualiza o texto do nível
            SetMaxExp(50); // Incrementa MAX_EXP em 10
        }
    }

    public float GetExp() => startExp;

    public void SetMaxExp(int exp) => MAX_EXP += exp;

    public void EXP(int exp) => startExp += exp; // Incrementa a experiência

    private void UpdateLevelBar()
    {
        levelBar.fillAmount = (float)startExp / MAX_EXP; // Atualiza a barra de nível com a proporção da experiência atual
    }
}
