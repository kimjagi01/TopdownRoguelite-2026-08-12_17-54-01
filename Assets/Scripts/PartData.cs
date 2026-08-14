using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPart", menuName = "Game/Weapon Part Data")]
public class PartData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string partName;

    [SerializeField]
    [TextArea]
    private string description;

    [SerializeField] private Sprite icon;


    [Header("Restrictions")]
    [Tooltip("비어 있으면 모든 무기와 모든 슬롯에 사용할 수 있습니다.")]
    [SerializeField]
    private List<PartRestriction> restrictions
        = new List<PartRestriction>();


    [Header("Part")]
    [SerializeField] private GameObject partPrefab;


    public string PartName => partName;
    public string Description => description;
    public Sprite Icon => icon;

    public GameObject PartPrefab => partPrefab;


    /// <summary>
    /// 해당 파츠가 특정 무기의 특정 슬롯에 장착 가능한지 확인합니다.
    /// </summary>
    public bool IsAllowed(WeaponType weaponType, string slotId)
    {
        // 제한 조건이 하나도 없으면 모든 무기 / 모든 슬롯 허용
        if (restrictions == null || restrictions.Count == 0)
        {
            return true;
        }

        // 등록된 허용 조합 중 현재 무기 + 슬롯 조합이 있는지 확인
        foreach (PartRestriction restriction in restrictions)
        {
            if (restriction.WeaponType == weaponType &&
                restriction.SlotId == slotId)
            {
                return true;
            }
        }

        return false;
    }
}