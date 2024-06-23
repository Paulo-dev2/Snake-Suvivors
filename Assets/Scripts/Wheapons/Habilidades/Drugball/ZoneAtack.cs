using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAtack : MonoBehaviour
{
    private float damage; // Dano causado aos inimigos
    private Animator anim;
    private float terminedZone = 0.75f;

    public static ZoneAtack Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        damage = Snake.instance.GetForce();
        anim = GetComponent<Animator>();
    }
    public void SetDamage(float dmg) => damage = dmg / 2;
    public void SetTimeZone(float time) => terminedZone += time;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                anim.SetInteger("transition", 0);
                collision.GetComponent<Enemy>().Damage(damage);
                Destroy(gameObject, terminedZone);
                break;
            case "Boss":
                anim.SetInteger("transition", 0);
                collision.GetComponent<IBoss>().Damage(damage);
                Destroy(gameObject, terminedZone);
                break;
        }
    }
}
