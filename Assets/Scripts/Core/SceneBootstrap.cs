using UnityEngine;
using UnityEngine.SceneManagement;

namespace CosmicYarnCat.Core
{
    public class SceneBootstrap : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject GameSystemsPrefab; // Assign the GameSystems prefab here

        private void Awake()
        {
            // Check if GameManager exists. If not, we started directly in a level scene.
            if (GameManager.Instance == null)
            {
                Debug.Log("Bootstrap: No GameManager found. Spawning GameSystems...");
                if (GameSystemsPrefab != null)
                {
                    Instantiate(GameSystemsPrefab);
                }
                else
                {
                    Debug.LogError("Bootstrap: GameSystems Prefab is missing!");
                }
            }
            
            // Destroy self after bootstrapping
            Destroy(gameObject);
        }
    }
}
