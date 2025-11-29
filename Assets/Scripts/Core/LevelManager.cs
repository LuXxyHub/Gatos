using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace CosmicYarnCat.Core
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Game Configuration")]
        public List<MoonData> AllMoons;
        public string HubSceneName = "HubScene";

        // Track collected moons by ID
        private HashSet<string> _collectedMoons = new HashSet<string>();

        public event Action<MoonData> OnMoonCollected;
        public event Action OnAllMoonsCollected;

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

        public void LoadMoon(MoonData moon)
        {
            Debug.Log($"Traveling to {moon.MoonName}...");
            SceneManager.LoadScene(moon.SceneName);
        }

        public void ReturnToHub()
        {
            Debug.Log("Returning to Hub...");
            SceneManager.LoadScene(HubSceneName);
        }

        public void CompleteMoon(MoonData moon)
        {
            if (!_collectedMoons.Contains(moon.MoonID))
            {
                _collectedMoons.Add(moon.MoonID);
                OnMoonCollected?.Invoke(moon);
                Debug.Log($"Moon Collected: {moon.MoonName} ({_collectedMoons.Count}/9)");

                if (_collectedMoons.Count >= 9)
                {
                    TriggerEndgame();
                }
            }
        }

        public bool IsMoonCompleted(MoonData moon)
        {
            return _collectedMoons.Contains(moon.MoonID);
        }

        private void TriggerEndgame()
        {
            Debug.Log("ALL 9 MOONS COLLECTED! SUMMONING GIANT LUNAR YARN CAT!");
            OnAllMoonsCollected?.Invoke();
            // Logic to play cutscene or load endgame scene
        }
    }
}
