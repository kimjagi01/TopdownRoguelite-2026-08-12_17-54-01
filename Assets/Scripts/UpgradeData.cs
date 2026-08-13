using UnityEngine;

public enum UpgradeType
{
    MoveSpeed,
    AttackDamage,
    MaxHealth,

    NewWeapon,
    WeaponUpgrade,
    NewPart,
    PartUpgrade
}

[CreateAssetMenu(
    fileName = "Upgrade_",
    menuName = "TopdownRoguelite/Upgrade Data"
)]
public class UpgradeData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string upgradeName;

    [TextArea(2, 4)]
    [SerializeField] private string description;

    [SerializeField] private UpgradeType upgradeType;

    [Header("Random Value")]
    [SerializeField] private float minValue = 1f;
    [SerializeField] private float maxValue = 1f;
    [SerializeField] private UpgradeRarity rarity = UpgradeRarity.Common;
    [SerializeField] private float weight = 60f;
    public string UpgradeName => upgradeName;
    public string Description => description;
    public UpgradeType UpgradeType => upgradeType;

    public float MinValue => minValue;
    public float MaxValue => maxValue;
    public UpgradeRarity Rarity => rarity;
    public float Weight => weight;
    public float RollValue()
    {
        if (maxValue < minValue)
        {
            return minValue;
        }

        return Random.Range(minValue, maxValue);
    }
}