using UnityEngine;

namespace CosmicYarnCat.World
{
    public enum InteractionType
    {
        ElasticPlatform,
        KnotTrap,
        WovenBridge
    }

    public class InteractiveElement : MonoBehaviour
    {
        public InteractionType Type;
        public float InteractionForce = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HandleInteraction(other.gameObject);
            }
        }

        private void HandleInteraction(GameObject player)
        {
            switch (Type)
            {
                case InteractionType.ElasticPlatform:
                    BouncePlayer(player);
                    break;
                case InteractionType.KnotTrap:
                    TrapPlayer(player);
                    break;
                case InteractionType.WovenBridge:
                    UnfoldBridge();
                    break;
            }
        }

        private void BouncePlayer(GameObject player)
        {
            var rb = player.GetComponent<CharacterController>();
            // CharacterController doesn't have AddForce, so we need to communicate with PlayerController
            // For now, let's assume PlayerController has a public method or we use SendMessage
            player.SendMessage("ApplyBoost", InteractionForce, SendMessageOptions.DontRequireReceiver);
        }

        private void TrapPlayer(GameObject player)
        {
            // Slow down player
            Debug.Log("Player Trapped in Knot!");
        }

        private void UnfoldBridge()
        {
            // Play animation
            Debug.Log("Bridge Unfolding...");
        }
    }
}
