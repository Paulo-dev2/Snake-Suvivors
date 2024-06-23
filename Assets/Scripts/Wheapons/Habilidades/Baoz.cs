using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baoz : MonoBehaviour, IWeapon
{
    private Rigidbody2D rig;
    private float speed = 15;
    private float damage;
    public bool isRight;
    private Animator anim;
    private int level;
    private float damageExtra;

    public static Baoz instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        damage = Snake.instance.GetForce();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damageExtra = 6.5f;
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        rig.velocity = isRight ? Vector2.right * speed : Vector2.left * speed;
    }

    public void Fire(Transform firePoint, bool isRight) => this.isRight = isRight; 
    private IEnumerator PlayAnimationAndDestroy(float delay)
    {
        anim.SetTrigger("hit");
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void IncrementLevel()
    {
        if (level == 5) return;
        level++;
        damage += damageExtra;
    }

    public int GetLevel() => this.level;
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
