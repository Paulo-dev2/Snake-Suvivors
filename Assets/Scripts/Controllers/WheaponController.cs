using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheaponController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public GameObject objRay;
    [SerializeField] public GameObject objBool;
    [SerializeField] public GameObject objDrugball;
    [SerializeField] public GameObject objThunderEffects;
    [SerializeField] public GameObject objThunder;
    [SerializeField] public GameObject objBaoz;
    [SerializeField] public GameObject objBoolFireRay;

    public static WheaponController instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void DesativeAll()
    {
        objRay.SetActive(false);
        objBool.SetActive(false);
        objThunderEffects.SetActive(false);
        objThunder.SetActive(false);
        objDrugball.SetActive(false);
        objBaoz.SetActive(false);
        objBoolFireRay.SetActive(false);
    }

    public void ActiveRay() => objRay.SetActive(true);
    public void ActiveBool() => objBool.SetActive(true);
    public void ActiveThunderEffects() => objThunderEffects.SetActive(true);
    public void ActiveThunder() => objThunder.SetActive(true);
    public void ActiveobjDrugBall() => objDrugball.SetActive(true);
    public void ActiveBaoz() => objBaoz.SetActive(true);
    public void ActiveBoolFireRay() => objBoolFireRay.SetActive(true);
}
