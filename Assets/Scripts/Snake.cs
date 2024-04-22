using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private float speed = 6;
    float horizontal = 0;
    float vertical = 0;
    bool isWalking = false;

    [SerializeField] private Transform applePrefab;
    [SerializeField] private Transform appleInGame;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        appleInGame = SpawnApple();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PositionCheck();
    }

    void Move()
    {
        // Se não pressionar nada o valor é 0, se pressionar para direta vai até o 1 e se pressionar esquerda vai até -1
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isWalking = (horizontal != 0 || vertical != 0);
        animator.SetBool("isWalk", false);
        if (isWalking )
        {
            var move = new Vector3(horizontal, vertical, 0).normalized;
            transform.position += move * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
        }

    }

    private void PositionCheck()
    {
        if (appleInGame != null && transform.position == appleInGame.position)
        {
            Destroy(appleInGame.gameObject);

            appleInGame = SpawnApple();
        }
    }

    private Transform SpawnApple() => Instantiate(applePrefab, new Vector3(UnityEngine.Random.Range(-9, 9), UnityEngine.Random.Range(-5, 5), 0), Quaternion.identity);

}
