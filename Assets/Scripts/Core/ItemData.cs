using UnityEngine;

namespace CosmicYarnCat.Core
{
    public enum ItemType
    {
        HealthPotion,
        EnergyPotion,
        StrengthBooster,
        YarnBundle
    }

    [CreateAssetMenu(fileName = "NewItem", menuName = "CosmicYarnCat/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string ItemName;
        public ItemType Type;
        public int Value; // Amount to heal/restore
        public GameObject PickupPrefab;
    }
}
