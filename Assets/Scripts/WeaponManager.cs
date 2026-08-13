using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Starting Weapon")]
    [SerializeField] private WeaponData startingWeapon;

    private WeaponInstance currentWeapon;

    public WeaponInstance CurrentWeapon => currentWeapon;

    private void Awake()
    {
        if (startingWeapon != null)
        {
            EquipWeapon(startingWeapon);
        }
        else
        {
            Debug.LogWarning(
                "WeaponManager: Starting Weapon is not assigned."
            );
        }
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if (weaponData == null)
        {
            Debug.LogWarning(
                "WeaponManager: Cannot equip a null weapon."
            );

            return;
        }

        currentWeapon = new WeaponInstance(weaponData);

        Debug.Log(
            $"Equipped Weapon: {weaponData.WeaponName}"
        );
    }

    public int GetAttackDamage()
    {
        if (currentWeapon == null)
        {
            return 0;
        }

        return currentWeapon.AttackDamage;
    }

    public float GetAttackSpeed()
    {
        if (currentWeapon == null)
        {
            return 0f;
        }

        return currentWeapon.AttackSpeed;
    }

    public float GetAttackRange()
    {
        if (currentWeapon == null)
        {
            return 0f;
        }

        return currentWeapon.AttackRange;
    }

    public void IncreaseAttackDamage(int amount)
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning(
                "WeaponManager: No weapon equipped."
            );

            return;
        }

        currentWeapon.IncreaseAttackDamage(amount);
    }

    public void IncreaseAttackSpeed(float amount)
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning(
                "WeaponManager: No weapon equipped."
            );

            return;
        }

        currentWeapon.IncreaseAttackSpeed(amount);
    }

    public void IncreaseAttackRange(float amount)
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning(
                "WeaponManager: No weapon equipped."
            );

            return;
        }

        currentWeapon.IncreaseAttackRange(amount);
    }
}