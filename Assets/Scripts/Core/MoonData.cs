using UnityEngine;

namespace CosmicYarnCat.Core
{
    [CreateAssetMenu(fileName = "NewMoonData", menuName = "CosmicYarnCat/MoonData")]
    public class MoonData : ScriptableObject
    {
        public string MoonName;
        [TextArea] public string Description;
        public string SceneName; // The name of the Unity Scene to load
        public GameObject BossPrefab;
        public Color ThemeColor = Color.white;
        
        // Unique ID to track completion
        public string MoonID; 
    }
}
