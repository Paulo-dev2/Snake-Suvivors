using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private Rigidbody2D rig;
    private float speed = 15;
    public float damage;
    public bool isRight;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 1f);
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
        // Toca a anima��o
        anim.SetTrigger("hit");
        // Aguarda um per�odo de tempo determinado pelo delay
        yield return new WaitForSeconds(delay);

        // Destr�i o objeto
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.GetComponent<Enemy>().Damage(damage);
                StartCoroutine(PlayAnimationAndDestroy(0.75f));
                break;
            case "Boss":
                collision.GetComponent<Boss>().Damage(damage);
                StartCoroutine(PlayAnimationAndDestroy(0.75f));
                break;
        }
    }
}
