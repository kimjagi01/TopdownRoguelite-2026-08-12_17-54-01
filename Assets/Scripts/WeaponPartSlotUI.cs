using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponPartSlotUI : MonoBehaviour, IDropHandler
{
    [Header("Slot")]
    [SerializeField] private string slotId;

    [Header("Weapon Manager")]
    [SerializeField] private WeaponManager weaponManager;

    [Header("UI")]
    [SerializeField] private Image partIcon;


    public string SlotId => slotId;


    private void Start()
    {
        Refresh();
    }


    public void Refresh()
    {
        // WeaponManager가 없으면 아이콘 숨김
        if (weaponManager == null)
        {
            Debug.LogWarning(
                $"WeaponPartSlotUI: WeaponManager is not assigned. ({slotId})"
            );

            if (partIcon != null)
            {
                partIcon.enabled = false;
            }

            return;
        }


        // 현재 장착된 무기 가져오기
        WeaponInstance currentWeapon = weaponManager.CurrentWeapon;

        if (currentWeapon == null)
        {
            if (partIcon != null)
            {
                partIcon.enabled = false;
            }

            return;
        }


        // 현재 슬롯에 장착된 파츠 가져오기
        PartData equippedPart =
            currentWeapon.GetEquippedPart(slotId);


        // 장착된 파츠가 없음
        if (equippedPart == null)
        {
            if (partIcon != null)
            {
                partIcon.enabled = false;
            }

            return;
        }


        // 장착된 파츠 아이콘 표시
        if (partIcon != null)
        {
            partIcon.sprite = equippedPart.Icon;
            partIcon.enabled = equippedPart.Icon != null;
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"④ {slotId} - OnDrop");


        // 드래그한 오브젝트에서 PartItemUI 찾기
        PartItemUI part =
            eventData.pointerDrag?.GetComponent<PartItemUI>();

        if (part == null)
        {
            Debug.LogWarning(
                "WeaponPartSlotUI: Dropped object does not have PartItemUI."
            );

            return;
        }


        Debug.Log(
            $"⑤ {part.PartData.PartName} detected!"
        );


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
        WeaponInstance currentWeapon =
            weaponManager.CurrentWeapon;

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

            // 데이터가 변경되었으므로 UI 갱신
            Refresh();
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