using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        public GameObject ButtonPrefab;
        public Transform ButtonContainer;

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            // Clear existing buttons
            foreach (Transform child in ButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // Create buttons for each moon
            foreach (var moon in LevelManager.Instance.AllMoons)
            {
                GameObject btnObj = Instantiate(ButtonPrefab, ButtonContainer);
                var btn = btnObj.GetComponent<Button>();
                var text = btnObj.GetComponentInChildren<TextMeshProUGUI>();

                bool isCompleted = LevelManager.Instance.IsMoonCompleted(moon);
                
                text.text = moon.MoonName + (isCompleted ? " [DONE]" : "");
                
                if (isCompleted)
                {
                    btn.interactable = false; // Or keep it replayable
                }
                else
                {
                    btn.onClick.AddListener(() => LevelManager.Instance.LoadMoon(moon));
                }
            }
        }
    }
}
