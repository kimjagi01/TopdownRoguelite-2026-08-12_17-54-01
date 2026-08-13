using UnityEngine;

[System.Serializable]
public class WeaponPartSlot
{
    [SerializeField] private string slotId;
    [SerializeField] private Transform attachmentPoint;

    public string SlotId => slotId;
    public Transform AttachmentPoint => attachmentPoint;

    public WeaponPartSlot(string id)
    {
        slotId = id;
    }
}