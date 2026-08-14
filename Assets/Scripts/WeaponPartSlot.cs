using UnityEngine;

[System.Serializable]
public class WeaponPartSlot
{
    [SerializeField] private string slotId;

    public string SlotId => slotId;

    public WeaponPartSlot(string id)
    {
        slotId = id;
    }
}