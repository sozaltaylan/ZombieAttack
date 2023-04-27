using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets / WeaponData")]
public class WeaponData : ScriptableObject
{
    public float attackPower;
    public float duration;
    public float jumpPower;
}
