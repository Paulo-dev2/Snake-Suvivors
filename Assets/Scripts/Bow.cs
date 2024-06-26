using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    private Rigidbody2D rig;
    private float speed = 10;
    public float damage;
    public bool isRight;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRight)
        {
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            rig.velocity = Vector2.left * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.GetComponent<Enemy>().Damage(damage);
                Destroy(gameObject);
                break;
            case "Boss":
                collision.GetComponent<Boss>().Damage(damage);
                Destroy(gameObject);
                break;
        }
    }
}
