using UnityEngine;

[System.Serializable]
public class WeaponInstance
{
    private WeaponData data;

    private int attackDamage;
    private float attackSpeed;
    private float attackRange;

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