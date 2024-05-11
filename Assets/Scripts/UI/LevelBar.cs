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
        // Se a experi�ncia for maior ou igual � experi�ncia m�xima, passa de n�vel
        UpdateLevelBar();
        if (startExp >= MAX_EXP)
        {
            level++; // Incrementa o n�vel
            startExp = 0; // Remove a experi�ncia necess�ria para passar de n�vel
            Level.text = level.ToString(); // Atualiza o texto do n�vel
            SetMaxExp(50); // Incrementa MAX_EXP em 10
        }
    }

    public float GetExp() => startExp;

    public void SetMaxExp(int exp) => MAX_EXP += exp;

    public void EXP(int exp) => startExp += exp; // Incrementa a experi�ncia

    private void UpdateLevelBar()
    {
        levelBar.fillAmount = (float)startExp / MAX_EXP; // Atualiza a barra de n�vel com a propor��o da experi�ncia atual
    }
}
