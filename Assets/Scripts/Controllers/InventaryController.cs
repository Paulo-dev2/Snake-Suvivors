using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class InventaryController : MonoBehaviour
{
    [SerializeField] private TMP_Text lifeCurrent;
    [SerializeField] private TMP_Text speedCurrent;
    [SerializeField] private TMP_Text forceCurrent;
    [SerializeField] private Sprite spritePrincipal;

    public Image[] slotsImagens;
    public GameObject[] starContainers; // Array de GameObjects que contêm as estrelas

    private List<Sprite> sprites = new List<Sprite>();
    private Dictionary<Sprite, int> spriteQuantidades = new Dictionary<Sprite, int>();
    private bool isUpdate;

    private Snake snake;

    public static InventaryController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake").GetComponent<Snake>();
        if(spritePrincipal != null )
        {
            sprites.Add(spritePrincipal);
            spriteQuantidades.Add(spritePrincipal, 0);
            UpdateStars(spritePrincipal, spriteQuantidades[spritePrincipal]);
            ActivateItem(spritePrincipal);
            isUpdate = true;
        }
    }

    private void Update()
    {
        lifeCurrent.text = snake.GetLife().ToString();
        forceCurrent.text = snake.GetForce().ToString();
        speedCurrent.text = snake.GetVelocity().ToString();
        if (isUpdate) UpdateUI();
    }

    public void AddItemToInventory(Sprite sprite)
    {
        if (spriteQuantidades.ContainsKey(sprite))
        {
            spriteQuantidades[sprite]++;
            UpdateStars(sprite, spriteQuantidades[sprite]);
            UpdateLevel(sprite);
        }
        else
        {
            sprites.Add(sprite);
            spriteQuantidades.Add(sprite, 0);
            UpdateStars(sprite, spriteQuantidades[sprite]);
            ActivateItem(sprite);
        }

        isUpdate = true;
    }

    private void UpdateLevel(Sprite sprite)
    {
        var spriteName = sprite.name;
        switch (spriteName)
        {
            case "Ray":
                Ray.instance.IncrementLevel();
                break;
            case "veneno":
                Poison.instance.IncrementLevel();
                break;
            case "bool":
                Bool.instance.IncrementLevel();
                break;
            case "thunder":
                Thunder.instance.IncrementLevel();
                break;
            case "baoz":
                Baoz.instance.IncrementLevel();
                break;
            case "5398538_2":
                BoolFire.instance.IncrementLevel();
                break;
            case "7938053_0":
                Thunder.instance.IncrementLevel();
                break;
            case "Fz_DrugPurple_0":
                AtackDrugball.instance.IncrementLevel();
                break;
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotsImagens.Length; i++)
        {
            if (i < sprites.Count)
            {
                slotsImagens[i].sprite = sprites[i];
                slotsImagens[i].gameObject.SetActive(true);
            }
            else
            {
                slotsImagens[i].sprite = null;
                slotsImagens[i].gameObject.SetActive(false);

            }
        }

        isUpdate = false;
    }

    private void UpdateStars(Sprite image, int countStars)
    {
        // Encontra o índice da imagem na lista de imagens
        if (countStars > 5 || countStars == 0) return;

        var spriteName = image.name;
        

        int index = sprites.IndexOf(image);

        // Acessa o contêiner de estrelas correspondente ao índice da imagem
        GameObject starContainer = starContainers[index];
        int childCount = starContainer.transform.childCount;

        // Cor quando a estrela está ativada
        Color activatedColor = new Color(1f, 1f, 1f); // Cor branca

        // Itera sobre os filhos do contêiner de estrelas
        for (int i = 0; i < childCount; i++)
        {
            // Obtém o componente Image da estrela
            Image starImage = starContainer.transform.GetChild(i).GetComponent<Image>();

            // Define a cor da estrela com base na quantidade de estrelas ativadas
            if (i < countStars)
            {
                starImage.color = activatedColor;
            }
        }

    }

    private void ActivateItem(Sprite sprite)
    {
        var spriteName = sprite.name;
        switch (spriteName)
        {
            case "Ray":
                WheaponController.instance.ActiveRay();
                break;
            case "bool":
                WheaponController.instance.ActiveBool();
                break;
            case "thunder":
                WheaponController.instance.ActiveThunder();
                break;
            case "baoz":
                WheaponController.instance.ActiveBaoz();
                break;
            case "5398538_2":
                WheaponController.instance.ActiveBoolFireRay();
                break;
            case "7938053_0":
                WheaponController.instance.ActiveThunder();
                break;
            case "Fx_DrugPurple_0":
                WheaponController.instance.ActiveobjDrugBall();
                break;
        }
    }
}
