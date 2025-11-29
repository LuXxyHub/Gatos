using UnityEngine;
using System;

namespace CosmicYarnCat.Core
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        public int StellarThread { get; private set; } // Main currency
        public int MoonFragments { get; private set; } // Key items

        public event Action<int> OnThreadChanged;
        public event Action<int> OnFragmentsChanged;

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

        public void AddThread(int amount)
        {
            StellarThread += amount;
            OnThreadChanged?.Invoke(StellarThread);
        }

        public bool SpendThread(int amount)
        {
            if (StellarThread >= amount)
            {
                StellarThread -= amount;
                OnThreadChanged?.Invoke(StellarThread);
                return true;
            }
            return false;
        }

        public void CollectFragment()
        {
            MoonFragments++;
            OnFragmentsChanged?.Invoke(MoonFragments);
        }
    }
}
