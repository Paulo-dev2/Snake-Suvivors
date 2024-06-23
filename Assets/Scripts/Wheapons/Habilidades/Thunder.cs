using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Thunder : MonoBehaviour, IWeapon
{
    private Rigidbody2D rig;
    private float speed = 15;
    public float damage;
    public bool isRight;
    private Animator anim;
    private int level;
    private float damageExtra;
    private float destroyThunder;
    // Start is called before the first frame update

    public static Thunder instance;
    public UnityEvent ThunderPassedLevel;
    private void Awake()
    {
        destroyThunder = 2f;
        instance = this;
    }
    void Start()
    {
        damage = Snake.instance.GetForce();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damageExtra = 8;
        Destroy(gameObject, destroyThunder);
    }

    public void Fire(Transform firePoint, bool isRight) => this.isRight = isRight;

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
        //anim.SetTrigger("hit");
        // Aguarda um período de tempo determinado pelo delay
        yield return new WaitForSeconds(delay);

        // Destrói o objeto
        Destroy(gameObject);
    }

    public void IncrementLevel()
    {
        if (level == 5) return;
        level++;
        damage += damageExtra;
        destroyThunder -= 0.20f;
        ThunderPassedLevel?.Invoke();
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
