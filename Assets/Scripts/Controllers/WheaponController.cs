using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheaponController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public GameObject objRay;
    [SerializeField] public GameObject objBool;
    [SerializeField] public GameObject objBoolFire;
    [SerializeField] public GameObject objThunder;

    public static WheaponController instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void DesativeAll()
    {
        objRay.SetActive(false);
        objBool.SetActive(false);
        objThunder.SetActive(false);
        objBoolFire.SetActive(false);
    }

    public void ActiveRay() => objRay.SetActive(true);
    public void ActiveBool() => objBool.SetActive(true);
    public void ActiveThunder() => objThunder.SetActive(true);
    public void ActiveBoolFire() => objBoolFire.SetActive(true);
}
