using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private Rigidbody2D rig;
    private float speed = 15;
    public float damage;
    public bool isRight;
    public int level;
    private Animator anim;
    private float damageExtra;
    // Start is called before the first frame update

    public static Poison instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        damage = Snake.instance.GetForce();
        damageExtra = 2f;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 2f);
    }

    public int Getlevel() => this.level;
    public void IncrementLevel()
    {
        if (level == 5) return;
        level++;
        damage += damageExtra;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRight)
        {
            anim.SetInteger("transition", 0);
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            anim.SetInteger("transition", 0);
            rig.velocity = Vector2.left * speed;
        }
    }

    private IEnumerator PlayAnimationAndDestroy(float delay)
    {
        // Toca a animação
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
                collision.GetComponent<IBoss>().Damage(damage);
                StartCoroutine(PlayAnimationAndDestroy(0.75f));
                break;
        }
    }
}
