using UnityEngine;
using UnityEngine.UI;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [Header("References")]
        public Image FillImage;
        public Health TargetHealth;
        public Canvas CanvasGroup; // To hide/show

        private void Start()
        {
            if (TargetHealth == null)
                TargetHealth = GetComponentInParent<Health>();

            if (TargetHealth != null)
            {
                TargetHealth.OnHealthChanged += UpdateHealth;
                TargetHealth.OnDeath += OnDeath;
                UpdateHealth(TargetHealth.CurrentHealth, TargetHealth.MaxHealth);
            }
            
            // Look at camera
            var canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.worldCamera = UnityEngine.Camera.main;
            }
        }

        private void LateUpdate()
        {
            // Billboard effect: Always face camera
            transform.LookAt(transform.position + UnityEngine.Camera.main.transform.rotation * Vector3.forward,
                             UnityEngine.Camera.main.transform.rotation * Vector3.up);
        }

        private void UpdateHealth(int current, int max)
        {
            if (FillImage != null)
            {
                FillImage.fillAmount = (float)current / max;
            }
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
