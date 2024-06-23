using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheaponDrugball : MonoBehaviour
{
    public GameObject habilidadePrefab; // Prefab da habilidade
    public Transform firePoint; // Ponto de origem do disparo
    private GameObject snake; // Referência ao objeto da cobra

    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake");
        StartCoroutine(SpawnWheapon());
    }

    private IEnumerator SpawnWheapon()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Fire();
        }
    }

    public void Fire()
    {
        float snakeRotation = snake.transform.rotation.y;
        bool isRight = snakeRotation == 0;

        // Instancia o prefab da habilidade
        GameObject habilidadeInstance = Instantiate(habilidadePrefab, firePoint.position, firePoint.rotation);

        // Obtém o componente IWeapon da habilidade instanciada
        IWeapon habilidadeWeapon = habilidadeInstance.GetComponent<IWeapon>();

        // Verifica se o componente IWeapon foi encontrado
        if (habilidadeWeapon != null)
        {
            // Chama o método Fire da habilidade
            habilidadeWeapon.Fire(firePoint, isRight);
        }
    }
}

