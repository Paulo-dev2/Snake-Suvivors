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
            Thunder.instance.ThunderPassedLevel.AddListener(OnThunderPassedLevel); // Verificar se Thunder.instance n�o � nulo
        StartCoroutine(SpawnWheapon());
    }

    private void OnThunderPassedLevel()
    {
        if (spawnThunder > 0.5f) // Adicionar um valor m�nimo para spawnThunder
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
        // Use a posi��o do firePoint e sua rota��o
        GameObject thunderObj = Instantiate(thunder, firePoint.position, firePoint.rotation);
        // Se precisar acessar algum componente do objeto, voc� pode fazer assim:
        Thunder thunderComponet = thunderObj.GetComponent<Thunder>();
        thunderComponet.isRight = snakeRotation == 0;
    }
}
