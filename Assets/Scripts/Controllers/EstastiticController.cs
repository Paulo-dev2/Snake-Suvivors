using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstastiticController : MonoBehaviour
{
    public Image[] slotsImagens;
    public TMP_Text[] slotNames;
    public TMP_Text[] slotDamages;

    private List<Sprite> sprites = new List<Sprite>();
    private List<string> names = new List<string>();
    private List<float> damages = new List<float>();

    private bool isUpdate;
    [SerializeField] private Sprite spritePrincipal;

    public static EstastiticController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (spritePrincipal != null)
        {
            sprites.Add(spritePrincipal);
            names.Add("Nome da Arma Principal"); // Adicione o nome da arma principal aqui
            damages.Add(10f); // Adicione o dano da arma principal aqui
        }
    }

    void Update()
    {
        if (isUpdate) UpdateUI();
    }

    public void AddWeaponToStatistics(Sprite sprite, string name, float damage)
    {
        sprites.Add(sprite);
        names.Add(name);
        damages.Add(damage);
        isUpdate = true;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotsImagens.Length; i++)
        {
            if (i < sprites.Count)
            {
                slotsImagens[i].sprite = sprites[i];
                slotNames[i].text = names[i];
                slotDamages[i].text = damages[i].ToString();
                slotsImagens[i].gameObject.SetActive(true);
            }
            else
            {
                slotsImagens[i].sprite = null;
                slotNames[i].text = "";
                slotDamages[i].text = "";
                slotsImagens[i].gameObject.SetActive(false);
            }
        }

        isUpdate = false;
    }
}
