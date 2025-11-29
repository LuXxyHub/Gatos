using UnityEngine;

namespace CosmicYarnCat.Core
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation)
        {
            if (vfxPrefab != null)
            {
                Instantiate(vfxPrefab, position, rotation);
                // Note: The VFX prefab should have a script to destroy itself (e.g., ParticleSystemAutoDestroy)
            }
        }

        public void SpawnVFX(GameObject vfxPrefab, Vector3 position)
        {
            SpawnVFX(vfxPrefab, position, Quaternion.identity);
        }
    }
}
