using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private int slotIndex;

    [Header("Part UI")]
    [SerializeField] private PartItemUI partItemPrefab;

    private PartItemUI currentPartItem;


    public int SlotIndex => slotIndex;


    private void Start()
    {
        Refresh();
    }


    public void Refresh()
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning(
                $"InventorySlotUI: InventoryManager is not assigned. " +
                $"({gameObject.name})"
            );

            return;
        }


        PartData partData =
            inventoryManager.GetPart(slotIndex);


        // 현재 슬롯에 파츠가 없다면 UI 제거
        if (partData == null)
        {
            if (currentPartItem != null)
            {
                Destroy(currentPartItem.gameObject);
                currentPartItem = null;
            }

            return;
        }


        // 이미 UI가 존재한다면 새로 만들 필요 없음
        if (currentPartItem != null)
        {
            return;
        }


        // PartItemUI 생성
        if (partItemPrefab == null)
        {
            Debug.LogWarning(
                $"InventorySlotUI: PartItemUI prefab is not assigned. " +
                $"({gameObject.name})"
            );

            return;
        }


        currentPartItem =
            Instantiate(partItemPrefab, transform);


        // 슬롯 중앙에 배치
        RectTransform itemRect =
            currentPartItem.GetComponent<RectTransform>();

        if (itemRect != null)
        {
            itemRect.anchoredPosition = Vector2.zero;
            itemRect.localScale = Vector3.one;
        }


        // PartData 연결
        currentPartItem.SetPartData(partData);


        Debug.Log(
            $"InventorySlotUI: Created {partData.PartName} " +
            $"→ Slot {slotIndex}"
        );
    }
}