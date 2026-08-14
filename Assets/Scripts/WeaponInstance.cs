using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInstance
{
    private WeaponData data;

    private int attackDamage;
    private float attackSpeed;
    private float attackRange;

    // 현재 장착된 파츠
    private Dictionary<string, PartData> equippedParts
        = new Dictionary<string, PartData>();


    public WeaponData Data => data;

    public int AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;


    public WeaponInstance(WeaponData weaponData)
    {
        data = weaponData;

        if (data == null)
        {
            return;
        }

        attackDamage = data.AttackDamage;
        attackSpeed = data.AttackSpeed;
        attackRange = data.AttackRange;
    }


    // 파츠 장착
    public bool EquipPart(PartData partData, string slotId)
    {
        if (partData == null)
        {
            Debug.LogWarning("WeaponInstance: Cannot equip a null part.");
            return false;
        }

        if (data == null)
        {
            Debug.LogWarning("WeaponInstance: Weapon data is null.");
            return false;
        }

        if (string.IsNullOrEmpty(slotId))
        {
            Debug.LogWarning("WeaponInstance: Slot ID is empty.");
            return false;
        }

        // 무기 + 슬롯 조합 확인
        if (!partData.IsAllowed(data.WeaponType, slotId))
        {
            Debug.LogWarning(
                $"WeaponInstance: {partData.PartName} cannot be equipped to " +
                $"{data.WeaponName} / {slotId}."
            );

            return false;
        }

        // 이미 해당 슬롯에 파츠가 있는지 확인
        if (equippedParts.ContainsKey(slotId))
        {
            Debug.LogWarning(
                $"WeaponInstance: Slot {slotId} is already occupied."
            );

            return false;
        }

        equippedParts.Add(slotId, partData);

        Debug.Log(
            $"[{data.WeaponName}] Equipped Part: {partData.PartName} → {slotId}"
        );

        return true;
    }


    // 파츠 해제
    public bool RemovePart(string slotId)
    {
        if (string.IsNullOrEmpty(slotId))
        {
            return false;
        }

        if (!equippedParts.ContainsKey(slotId))
        {
            Debug.LogWarning(
                $"WeaponInstance: No part equipped in {slotId}."
            );

            return false;
        }

        PartData removedPart = equippedParts[slotId];

        equippedParts.Remove(slotId);

        Debug.Log(
            $"[{data.WeaponName}] Removed Part: {removedPart.PartName} ← {slotId}"
        );

        return true;
    }


    // 특정 슬롯의 파츠 가져오기
    public PartData GetEquippedPart(string slotId)
    {
        if (string.IsNullOrEmpty(slotId))
        {
            return null;
        }

        if (equippedParts.TryGetValue(slotId, out PartData part))
        {
            return part;
        }

        return null;
    }


    // 파츠 개수
    public int GetEquippedPartCount()
    {
        return equippedParts.Count;
    }


    public void IncreaseAttackDamage(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        attackDamage += amount;

        Debug.Log(
            $"[{data.WeaponName}] Attack Damage: {attackDamage}"
        );
    }


    public void IncreaseAttackSpeed(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        attackSpeed += amount;
        attackSpeed = Mathf.Clamp(attackSpeed, 0f, 100f);

        Debug.Log(
            $"[{data.WeaponName}] Attack Speed: {attackSpeed}"
        );
    }


    public void IncreaseAttackRange(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        attackRange += amount;
        attackRange = Mathf.Clamp(attackRange, 0f, 100f);

        Debug.Log(
            $"[{data.WeaponName}] Attack Range: {attackRange}"
        );
    }
}