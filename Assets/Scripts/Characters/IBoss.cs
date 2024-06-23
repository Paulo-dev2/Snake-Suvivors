using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IBoss
{

    void CheckMovementDirection();

    public void DestroyBoss();

    public void Damage(float dmg);

    public void Fire();

    UnityEvent BossDestroyedEvent { get; }
}
