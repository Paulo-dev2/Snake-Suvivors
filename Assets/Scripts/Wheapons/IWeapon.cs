using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void IncrementLevel();
    public int GetLevel();
    void Fire(Transform firePoint, bool isRight);
}
