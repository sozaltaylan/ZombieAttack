using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FastZombie : BaseZombie
{
    protected override void SetZombieArise(Transform transform)
    {
        base.SetZombieArise(transform);
    }
    protected override void SetTarget(Transform transform, float zombieSpeed)
    {
        base.SetTarget(transform, _zombieData.fastZombieSpeed);
    }
    protected override void SetDead()
    {
        base.SetDead();
    }
    protected override void HandleStart()
    {
        base.HandleStart();
    }
    protected override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    protected override void SetHealth(float damage)
    {
        base.SetHealth(damage);
    }
    protected override void SetAttack()
    {
        base.SetAttack();
    }
    protected override void CreateDamageText(float damage, Vector3 pos)
    {
        base.CreateDamageText(damage, pos);
    }
}
