using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiseOptionController : MonoBehaviour
{
    public Item[] slots;
    public Image[] slotImage;
    public TMP_Text[] slotName;
    public Transform[] prefabs;

    void Start()
    {
        // Inicializa os slots no início do jogo
        //InitializeSlots();
    }

    void Update()
    {
        // Preenche os slots vazios com novos itens
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                AssignNewItemToSlot(i);
            }
        }
    }

    void AssignNewItemToSlot(int slotIndex)
    {
        // Seleciona um prefab aleatório da lista
        Transform itemPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
        var instantiatePrefab = itemPrefab.GetComponent<ItemType>().itemTypes;

        // Verifica se o item já está presente em outro slot
        bool itemExistsInOtherSlot = false;
        foreach (Item item in slots)
        {
            if (item != null && item.name == instantiatePrefab.name)
            {
                itemExistsInOtherSlot = true;
                break;
            }
        }

        // Se o item não existir em outro slot, atribui ao slot atual
        if (!itemExistsInOtherSlot)
        {
            //var name = slots[slotIndex].itemName;
            //var spriteItem = slots[slotIndex].spriteItem;
            slots[slotIndex] = instantiatePrefab;
            slotImage[slotIndex].sprite = slots[slotIndex].spriteItem;
            slotName[slotIndex].text = slots[slotIndex].itemName;

            // Adiciona um listener de clique à imagem do slot
            int index = slotIndex; // Salva o valor de slotIndex para ser usado dentro do listener
            Button button = slotImage[slotIndex].gameObject.GetComponent<Button>();
            if (button == null)
            {
                button = slotImage[slotIndex].gameObject.AddComponent<Button>();
            }
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnSlotClick(index));
        }
    }

    // Método chamado quando uma imagem de slot é clicada
    void OnSlotClick(int index)
    {
        Sprite clonedSprite = slotImage[index].sprite;
        // Lógica quando um item é clicado
        ScenesController.instance.OndPassed();
        InventaryController.instance.AddItemToInventory(clonedSprite);

        // Reseta os slots após a escolha
        ResetSlots();
    }

    void ResetSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = null;
            slotImage[i].sprite = null;
            Button button = slotImage[i].gameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
}
