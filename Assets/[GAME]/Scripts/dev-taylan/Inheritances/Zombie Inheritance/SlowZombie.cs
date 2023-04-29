using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SlowZombie : BaseZombie
{

    protected override void HandleStart()
    {
        base.HandleStart();
    }
    protected override void HandleUpdate()
    {
        base.HandleUpdate();
    }

    protected override void SetTarget(Transform transform, float zombieSpeed)
    {
        base.SetTarget(this.transform, _zombieData.zombieSpeed);
    }
    protected override void SetAttack(bool active)
    {
        base.SetAttack(active);
    }
    protected override void SetZombieArise(Transform transform)
    {
        base.SetZombieArise(this.transform);
    }
    protected override void SetDead()
    {
        base.SetDead();
    }
    protected override void SetHealth(float damage)
    {
        base.SetHealth(damage);
    }

    protected override void CreateDamageText(float damage,Vector3 pos)
    {
        base.CreateDamageText(damage,pos);
    }
}