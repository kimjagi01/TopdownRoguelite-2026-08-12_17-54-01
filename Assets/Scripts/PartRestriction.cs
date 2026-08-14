using System;
using UnityEngine;

[Serializable]
public class PartRestriction
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private string slotId;

    public WeaponType WeaponType => weaponType;
    public string SlotId => slotId;
}