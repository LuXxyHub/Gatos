using UnityEngine;

namespace CosmicYarnCat.World
{
    [CreateAssetMenu(fileName = "NewMoonBiome", menuName = "CosmicYarnCat/MoonBiome")]
    public class MoonBiome : ScriptableObject
    {
        public string BiomeName;
        public Color AmbientColor;
        public Color FogColor;
        public Material SkyboxMaterial;
        public GameObject[] EnemySpawns;
        public GameObject BossPrefab;
    }
}
