using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private int capacity = 5;

    [Header("Starting Parts")]
    [SerializeField]
    private List<PartData> startingParts =
        new List<PartData>();

    private List<PartData> parts = new List<PartData>();

    public int Capacity => capacity;
    public int Count => parts.Count;
    public bool IsFull => parts.Count >= capacity;


    private void Awake()
    {
        parts.Clear();

        for (int i = 0; i < capacity; i++)
        {
            parts.Add(null);
        }


        // 시작 파츠 등록
        for (int i = 0; i < startingParts.Count && i < capacity; i++)
        {
            if (startingParts[i] != null)
            {
                parts[i] = startingParts[i];

                Debug.Log(
                    $"Inventory: Starting Part added → " +
                    $"{startingParts[i].PartName} / Slot {i}"
                );
            }
        }
    }


    // 특정 슬롯의 파츠 가져오기
    public PartData GetPart(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= parts.Count)
        {
            return null;
        }

        return parts[slotIndex];
    }


    // 빈 슬롯에 파츠 추가
    public bool AddPart(PartData partData)
    {
        if (partData == null)
        {
            Debug.LogWarning(
                "InventoryManager: Cannot add a null part."
            );

            return false;
        }


        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i] == null)
            {
                parts[i] = partData;

                Debug.Log(
                    $"Inventory: Added {partData.PartName} → Slot {i}"
                );

                return true;
            }
        }


        Debug.LogWarning(
            $"Inventory: Inventory is full. Cannot add {partData.PartName}."
        );

        return false;
    }


    // 특정 슬롯의 파츠 제거
    public bool RemovePart(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= parts.Count)
        {
            return false;
        }


        if (parts[slotIndex] == null)
        {
            Debug.LogWarning(
                $"Inventory: Slot {slotIndex} is already empty."
            );

            return false;
        }


        Debug.Log(
            $"Inventory: Removed {parts[slotIndex].PartName} ← Slot {slotIndex}"
        );

        parts[slotIndex] = null;

        return true;
    }


    // 특정 슬롯에 파츠 설정
    public bool SetPart(int slotIndex, PartData partData)
    {
        if (slotIndex < 0 || slotIndex >= parts.Count)
        {
            return false;
        }

        parts[slotIndex] = partData;

        return true;
    }
}