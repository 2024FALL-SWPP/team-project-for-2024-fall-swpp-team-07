using System.Collections;
using UnityEngine;

namespace EasyDestuctibleWall
{
    public class DestructionManager : MonoBehaviour
    {
        // The hitpoints of the object, when this value is below 1, the chunk will fracture
        [SerializeField]
        private float health = 100f;

        // These two variables are used to multiply damage based on velocity and torque respectively.
        [SerializeField]
        private float impactMultiplier = 2.25f;

        [SerializeField]
        private float twistMultiplier = 0.0025f;

        private Rigidbody cachedRigidbody;

        private void Awake()
        {
            cachedRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            /**
            * Damage based on torque. When an object spins very fast, it is expected that this force will
            * tear it apart
            */
            health -= Mathf.Round(cachedRigidbody.angularVelocity.sqrMagnitude * twistMultiplier);

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
                health -= relativeVelocity * impactMultiplier * collision.rigidbody.mass;
            }
            else
            {
                health -= relativeVelocity * impactMultiplier;
            }
        }
    }
}
