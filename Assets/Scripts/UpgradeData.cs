using UnityEngine;

public enum UpgradeType
{
    MoveSpeed,
    AttackDamage,
    MaxHealth,

    // 추후 확장
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

    [Header("Value")]
    [SerializeField] private float value = 1f;

    public string UpgradeName => upgradeName;
    public string Description => description;
    public UpgradeType UpgradeType => upgradeType;
    public float Value => value;
}