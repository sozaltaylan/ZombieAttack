using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets / CharecterData")]
public class CharacterData : ScriptableObject
{
    public float attackDistance;
    public float attackTime;
    public float closeRangeAttackDistance;
    public float maxHealth;
}
