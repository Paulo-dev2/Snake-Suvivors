using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventaryController : MonoBehaviour
{
    [SerializeField] private TMP_Text lifeCurrent;
    [SerializeField] private TMP_Text speedCurrent;
    [SerializeField] private TMP_Text forceCurrent;

    public Image[] slotsImagens;

    public GameObject[] starContainers; // Array de GameObjects que contêm as estrelas

    private List<Image> imagens = new List<Image>();
    private Dictionary<Image, int> imagensQuantidade = new Dictionary<Image, int>();
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
    }

    private void Update()
    {
        lifeCurrent.text = snake.GetLife().ToString();
        forceCurrent.text = snake.GetForce().ToString();
        speedCurrent.text = snake.GetVelocity().ToString();
        if (isUpdate) UpdateUI();
    }

    public void AddItemToInventory(Image image)
    {
        if (imagensQuantidade.ContainsKey(image))
        {
            imagensQuantidade[image]++;
            UpdateStars(image, imagensQuantidade[image]);
        }
        else
        {
            imagens.Add(image);
            imagensQuantidade.Add(image, 1);
            UpdateStars(image, imagensQuantidade[image]);
            ActivateItem(image);
        }

        isUpdate = true;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotsImagens.Length; i++)
        {
            if (i < imagens.Count)
            {
                slotsImagens[i].sprite = imagens[i].sprite;
                Debug.Log(slotsImagens[i]);
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

    private void UpdateStars(Image image, int starsActivated)
    {
        // Encontra o índice da imagem na lista de imagens
        int index = imagens.IndexOf(image);

        // Acessa o contêiner de estrelas correspondente ao índice da imagem
        GameObject starContainer = starContainers[index];
        int childCount = starContainer.transform.childCount;

        // Cor padrão das estrelas
        Color defaultColor = Color.white;

        // Cor quando a estrela está ativada
        Color activatedColor = new Color(1f, 1f, 1f); // Cor branca

        // Itera sobre os filhos do contêiner de estrelas
        for (int i = 0; i < childCount; i++)
        {
            // Obtém o componente Image da estrela
            Image starImage = starContainer.transform.GetChild(i).GetComponent<Image>();

            // Define a cor da estrela com base na quantidade de estrelas ativadas
            if (i < starsActivated)
            {
                starImage.color = activatedColor;
            }
        }
    }

    private void ActivateItem(Image image)
    {
        var spriteName = image.sprite.name;
        switch (spriteName)
        {
            case "bool_ray":
                WheaponController.instance.ActiveRay();
                break;
            case "bool":
                WheaponController.instance.ActiveBool();
                break;

            case "thunder":
                WheaponController.instance.ActiveThunder();
                break;

            case "bool_fire":
                WheaponController.instance.ActiveBoolFire();
                break;
        }
    }
}
