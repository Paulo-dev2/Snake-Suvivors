using UnityEngine;
using UnityEngine.UI;

public class ChoiseOptionController : MonoBehaviour
{
    public Item[] slots;
    public Image[] slotImage;
    public Transform[] prefabs;

    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Verifica se o slot já está preenchido
            if (slots[i] == null)
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
                    slots[i] = instantiatePrefab;
                    slotImage[i].sprite = slots[i].spriteItem;

                    // Adiciona um listener de clique à imagem do slot
                    int index = i; // Salva o valor de i para ser usado dentro do listener
                    slotImage[i].gameObject.AddComponent<Button>().onClick.AddListener(() => OnSlotClick(index));
                }

                // Sai do loop, pois já preencheu um slot
                break;
            }
        }
    }

    // Método chamado quando uma imagem de slot é clicada
    void OnSlotClick(int index)
    {
        ScenesController.instance.OndPassed();
        InventaryController.instance.AddItemToInventory(this.slotImage[index]);
        // Aqui você pode adicionar a lógica para lidar com o clique, como abrir um menu de detalhes do item, etc.
    }
}
