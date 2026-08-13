using UnityEngine;

public enum WeaponType
{
    Sword,
    Spear,
    GreatSword,
    ShieldSword,
    Gun,
    Staff
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string weaponName;
    [SerializeField][TextArea] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private WeaponType weaponType;

    [Header("Weapon Stats")]
    [SerializeField] private int attackDamage = 10;

    // 0 ~ 100 사이의 무기 공격속도 능력치
    [SerializeField][Range(0f, 100f)] private float attackSpeed = 50f;

    // 0 ~ 100 사이의 무기 사거리 능력치
    [SerializeField][Range(0f, 100f)] private float attackRange = 50f;

    [Header("Part Slots")]
    [SerializeField]
    private string[] partSlots = new string[8]
    {
        "Handle",
        "Guard",
        "Blade_Left_Top",
        "Blade_Left_Middle",
        "Blade_Left_Bottom",
        "Blade_Right_Top",
        "Blade_Right_Middle",
        "Blade_Right_Bottom"
    };

    public string WeaponName => weaponName;
    public string Description => description;
    public Sprite Icon => icon;
    public WeaponType WeaponType => weaponType;

    public int AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;

    public string[] PartSlots => partSlots;
}