using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    public void SetText(float dmg)
    {
        damage.text = dmg.ToString();
    }
}
