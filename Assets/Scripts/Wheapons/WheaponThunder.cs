using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheaponThunder : MonoBehaviour
{
    public GameObject thunder;
    public Transform firePoint;
    private GameObject snake;
    private float spawnThunder;

    void Start()
    {
        spawnThunder = 2f;
        snake = GameObject.FindGameObjectWithTag("Snake");
        if (Thunder.instance != null)
            Thunder.instance.ThunderPassedLevel.AddListener(OnThunderPassedLevel); // Verificar se Thunder.instance não é nulo
        StartCoroutine(SpawnWheapon());
    }

    private void OnThunderPassedLevel()
    {
        if (spawnThunder > 0.5f) // Adicionar um valor mínimo para spawnThunder
        {
            spawnThunder -= 0.20f;
        }
    }

    private IEnumerator SpawnWheapon()
    {
        while (true)
        { 
            yield return new WaitForSeconds(spawnThunder);
            Fire();
        }
    }

    public void teste() => Debug.Log("Teste");

    public void Fire()
    {
        float snakeRotation = snake.transform.rotation.y;
        // Use a posição do firePoint e sua rotação
        GameObject thunderObj = Instantiate(thunder, firePoint.position, firePoint.rotation);
        // Se precisar acessar algum componente do objeto, você pode fazer assim:
        Thunder thunderComponet = thunderObj.GetComponent<Thunder>();
        thunderComponet.isRight = snakeRotation == 0;
    }
}
