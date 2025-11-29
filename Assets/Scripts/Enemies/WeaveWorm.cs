using UnityEngine;
using System.Collections;

namespace CosmicYarnCat.Enemies
{
    public class WeaveWorm : EnemyAI
    {
        [Header("Worm Settings")]
        public float BurrowDuration = 2f;
        public bool IsBurrowed { get; private set; }

        protected override void Update()
        {
            if (IsBurrowed)
            {
                // Wait or move underground
                // If close to player, unburrow
                if (Player != null && Vector3.Distance(transform.position, Player.position) < AttackRange)
                {
                    StartCoroutine(Unburrow());
                }
            }
            else
            {
                base.Update();
            }
        }

        protected override void PerformAttack()
        {
            StartCoroutine(Burrow());
        }

        private IEnumerator Burrow()
        {
            IsBurrowed = true;
            // Play burrow animation/particles
            // Disable collider/renderer
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            
            yield return new WaitForSeconds(BurrowDuration);
            
            // Move to random position near player?
        }

        private IEnumerator Unburrow()
        {
            // Play unburrow animation
            yield return new WaitForSeconds(0.5f);
            
            GetComponent<Collider>().enabled = true;
            GetComponent<Renderer>().enabled = true;
            IsBurrowed = false;
            
            // Deal damage if close
        }
    }
}
