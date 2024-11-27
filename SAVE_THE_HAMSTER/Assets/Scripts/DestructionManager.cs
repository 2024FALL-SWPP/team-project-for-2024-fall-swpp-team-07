using System.Collections;
using UnityEngine;

namespace EasyDestuctibleWall
{
    public class DestructionManager : MonoBehaviour
    {
        WallBehavior parentWallBehavior;

        // The hitpoints of the object, when this value is below 1, the chunk will fracture
        [SerializeField]
        private float health = 100f; // 100

        // These two variables are used to multiply damage based on velocity and torque respectively.
        [SerializeField]
        private float impactMultiplier = 2.25f; // 2.25

        [SerializeField]
        private float twistMultiplier = 0.0025f;

        private Rigidbody cachedRigidbody;

        private void Awake()
        {
            cachedRigidbody = GetComponent<Rigidbody>();
            parentWallBehavior = GetComponentInParent<WallBehavior>();
            health = 10;
        }

        private void FixedUpdate()
        {
            /**
            * Damage based on torque. When an object spins very fast, it is expected that this force will
            * tear it apart
            */
            if (parentWallBehavior.isBreakable)
            {
                cachedRigidbody.isKinematic = false;
                health -= Mathf.Round(
                    cachedRigidbody.angularVelocity.sqrMagnitude * twistMultiplier
                );
            }
            else
            {
                cachedRigidbody.isKinematic = true;
            }

            if (health <= 0f)
            {
                foreach (Transform child in transform)
                {
                    Rigidbody spawnRB = child.gameObject.AddComponent<Rigidbody>();
                    child.parent = null;
                    // Transfer velocity
                    spawnRB.velocity = GetComponent<Rigidbody>().GetPointVelocity(child.position);
                    // Transfer torque
                    spawnRB.AddTorque(
                        GetComponent<Rigidbody>().angularVelocity,
                        ForceMode.VelocityChange
                    );
                }
                Destroy(gameObject); // Destroy this now empty chunk object
            }
        }

        // When the chunk hits another object, take some of its health away
        void OnCollisionEnter(Collision collision)
        {
            float relativeVelocity = collision.relativeVelocity.sqrMagnitude;

            // If the chunk was hit by a rigidbody, multiply the damage by its mass
            if (collision.rigidbody)
            {
                if (parentWallBehavior.isBreakable)
                {
                    health -= relativeVelocity * impactMultiplier * collision.rigidbody.mass;
                }
            }
            else
            {
                if (parentWallBehavior.isBreakable)
                {
                    health -= relativeVelocity * impactMultiplier;
                }
            }
        }
    }
}
