using UnityEngine;
using CosmicYarnCat.Player;

namespace CosmicYarnCat.World
{
    public class CollectibleItem : MonoBehaviour
    {
        public CosmicYarnCat.Core.ItemData Data;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect(other.gameObject);
            }
        }

        private void Collect(GameObject player)
        {
            var stats = player.GetComponent<PlayerStats>();
            
            switch (Data.Type)
            {
                case CosmicYarnCat.Core.ItemType.HealthPotion:
                    stats.Heal(Data.Value);
                    break;
                case CosmicYarnCat.Core.ItemType.EnergyPotion:
                    // stats.RestoreEnergy(Data.Value);
                    break;
                case CosmicYarnCat.Core.ItemType.YarnBundle:
                    CosmicYarnCat.Core.ResourceManager.Instance.AddThread(Data.Value);
                    break;
            }

            Debug.Log($"Collected {Data.ItemName}!");
            Destroy(gameObject);
        }
    }
}
