using UnityEngine;

namespace CosmicYarnCat.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        public Transform Target;

        [Header("Settings")]
        public Vector3 Offset = new Vector3(-10, 10, -10); // Isometric angle
        public float SmoothSpeed = 0.125f;

        private void LateUpdate()
        {
            if (Target == null)
                return;

            Vector3 desiredPosition = Target.position + Offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(Target);
        }
    }
}
