    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AtackDrugball : MonoBehaviour, IWeapon
{
        private Rigidbody2D rig;
        private float speed = 15;
        private float damage;
        public bool isRight;
        private Animator anim;
        private int level = 1;
        private float damageExtra;

        public static AtackDrugball instance;
        public GameObject attackZonePrefab; // Prefab da zona de ataque

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            damage = Snake.instance.GetForce();
            rig = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            damageExtra = 2;
            Destroy(gameObject, 2f);
        }
        public void Fire(Transform firePoint, bool isRight) => this.isRight = isRight;
        void FixedUpdate()
        {
        
            if (isRight)
            {
                rig.velocity = Vector2.right * speed;
                transform.rotation = Quaternion.Euler(0, 0, -450f);
            }
            else
            {
                rig.velocity =  Vector2.left * speed;
                transform.rotation = Quaternion.Euler(0, 180, -450f);
            }
        }

        public void IncrementLevel()
        {
            if (level == 5) return;
            level++;
            damage += damageExtra;
            ZoneAtack.Instance.SetTimeZone(0.25f);
        }

        public int GetLevel() => this.level;

        public void ColliderZone(float delay)
        {
            Vector3 impactPosition = transform.position;
            GameObject attackZone = Instantiate(attackZonePrefab, impactPosition, Quaternion.identity);

            ZoneAtack zoneScript = attackZone.GetComponent<ZoneAtack>();

            if (zoneScript != null)
                zoneScript.SetDamage(damage);

            Destroy(gameObject, delay);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Enemy":
                    collision.GetComponent<Enemy>().Damage(damage);
                    ColliderZone(0.5f);
                    break;
                case "Boss":
                    collision.GetComponent<IBoss>().Damage(damage);
                    ColliderZone(0.5f);
                    break;
            }
        }
    }
