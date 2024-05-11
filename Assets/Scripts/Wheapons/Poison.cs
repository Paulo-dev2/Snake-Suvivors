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
    public int level = 1;
    private Animator anim;
    // Start is called before the first frame update

    public static Poison instance;

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
                collision.GetComponent<Boss>().Damage(damage);
                StartCoroutine(PlayAnimationAndDestroy(0.75f));
                break;
        }
    }
}
