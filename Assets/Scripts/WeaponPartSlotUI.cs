using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponPartSlotUI : MonoBehaviour, IDropHandler
{
    [Header("Slot")]
    [SerializeField] private string slotId;

    [Header("Weapon Manager")]
    [SerializeField] private WeaponManager weaponManager;


    public string SlotId => slotId;


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"④ {slotId} - OnDrop");

        // 드래그한 오브젝트에서 PartItemUI 찾기
        PartItemUI part = eventData.pointerDrag?.GetComponent<PartItemUI>();

        if (part == null)
        {
            Debug.LogWarning(
                "WeaponPartSlotUI: Dropped object does not have PartItemUI."
            );

            return;
        }

        Debug.Log("⑤ PartTest detected!");


        // PartData 가져오기
        PartData partData = part.PartData;

        if (partData == null)
        {
            Debug.LogWarning(
                "WeaponPartSlotUI: PartData is not assigned."
            );

            return;
        }


        // WeaponManager 확인
        if (weaponManager == null)
        {
            Debug.LogWarning(
                "WeaponPartSlotUI: WeaponManager is not assigned."
            );

            return;
        }


        // 현재 장착된 무기 가져오기
        WeaponInstance currentWeapon = weaponManager.CurrentWeapon;

        if (currentWeapon == null)
        {
            Debug.LogWarning(
                "WeaponPartSlotUI: No weapon is currently equipped."
            );

            return;
        }


        // 실제 파츠 장착
        bool success = currentWeapon.EquipPart(
            partData,
            slotId
        );


        if (success)
        {
            Debug.Log(
                $"Part equipped successfully: " +
                $"{partData.PartName} → " +
                $"{currentWeapon.Data.WeaponName} / {slotId}"
            );
        }
        else
        {
            Debug.LogWarning(
                $"Part equip failed: " +
                $"{partData.PartName} → " +
                $"{currentWeapon.Data.WeaponName} / {slotId}"
            );
        }
    }
}