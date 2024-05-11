using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explossion : MonoBehaviour
{
    private Rigidbody2D rig;
    private float speed = 15;
    public float damage;
    public bool isRight;
    private Animator anim;
    public int level = 1;

    public static Explossion instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        rig.velocity = isRight ? Vector2.right * speed : Vector2.left * speed;
    }

    private IEnumerator PlayAnimationAndDestroy(float delay)
    {
        anim.SetTrigger("hit");
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.GetComponent<Enemy>().Damage(damage);
                StartCoroutine(PlayAnimationAndDestroy(0.25f));
                break;
            case "Boss":
                collision.GetComponent<Boss>().Damage(damage);
                StartCoroutine(PlayAnimationAndDestroy(0.75f));
                break;
        }
    }
}
