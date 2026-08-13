using UnityEngine;

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

    [SerializeField]
    [Range(0f, 100f)]
    private float attackSpeed = 50f;

    [SerializeField]
    [Range(0f, 100f)]
    private float attackRange = 50f;

    [Header("Part Slots")]
    [SerializeField]
    private WeaponPartSlot[] partSlots = new WeaponPartSlot[8];

    public string WeaponName => weaponName;
    public string Description => description;
    public Sprite Icon => icon;
    public WeaponType WeaponType => weaponType;

    public int AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;

    public WeaponPartSlot[] PartSlots => partSlots;

    public WeaponPartSlot GetPartSlot(string slotId)
    {
        if (string.IsNullOrEmpty(slotId))
        {
            return null;
        }

        foreach (WeaponPartSlot slot in partSlots)
        {
            if (slot == null)
            {
                continue;
            }

            if (slot.SlotId == slotId)
            {
                return slot;
            }
        }

        return null;
    }
}