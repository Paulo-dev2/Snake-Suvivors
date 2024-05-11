using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheapon : MonoBehaviour
{
    public GameObject explosion;
    public Transform firePoint;
    private GameObject snake;

    void Start()
    {
        snake = GameObject.FindGameObjectWithTag("Snake");
        StartCoroutine(SpawnWheapon());
    }

    private IEnumerator SpawnWheapon()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.6f);
            Fire();
        }
    }

    public void Fire()
    {
        float snakeRotation = snake.transform.rotation.y;
        // Use a posi��o do firePoint e sua rota��o
        GameObject explosionObj = Instantiate(explosion, firePoint.position, firePoint.rotation);
        // Se precisar acessar algum componente do objeto, voc� pode fazer assim:
        Explossion explosionComponent = explosionObj.GetComponent<Explossion>();
        explosionComponent.isRight = snakeRotation == 0;
    }
}
