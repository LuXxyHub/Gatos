using UnityEngine;

namespace CosmicYarnCat.Core
{
    public class KeepAlive : MonoBehaviour
    {
        private void Awake()
        {
            // Make this object survive scene loads
            DontDestroyOnLoad(gameObject);
        }
    }
}
