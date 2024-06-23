using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Rigidbody2D rig;
    private float speed = 8;
    public float damage;
    public bool isRight;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.velocity = isRight ? Vector2.right * speed : Vector2.left * speed;
        anim.SetInteger("transition", 0);

        if (Mecha.instance != null) // Check if Mecha.instance is not null
        {
            transform.rotation = Mecha.instance.transform.rotation.y == 1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        }
    }

    private IEnumerator PlayAnimationAndDestroy(float delay)
    {

        // Aguarda um período de tempo determinado pelo delay
        yield return new WaitForSeconds(delay);

        // Destrói o objeto
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Snake")
        {
            collision.GetComponent<Snake>().Damage(damage);
            StartCoroutine(PlayAnimationAndDestroy(0.25f));
        }
    }
}
