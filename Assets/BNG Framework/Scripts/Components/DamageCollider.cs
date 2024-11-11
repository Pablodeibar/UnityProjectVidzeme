using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{

    public class DamageCollider : MonoBehaviour
    {

        public float Damage = 25f;
        public Rigidbody ColliderRigidbody;
        public float MinForce = 0.1f;
        public float LastRelativeVelocity = 0;
        public float LastDamageForce = 0;
        public bool TakeCollisionDamage = false;
        public float CollisionDamage = 5;
        Damageable thisDamageable;

        // Reference to the scoring system
        private ScoringSystem scoringSystem;

        private void Start()
        {
            if (ColliderRigidbody == null)
            {
                ColliderRigidbody = GetComponent<Rigidbody>();
            }

            thisDamageable = GetComponent<Damageable>();

            // Find the ScoringSystem in the scene
            scoringSystem = FindObjectOfType<ScoringSystem>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            OnCollisionEvent(collision);
        }

        public virtual void OnCollisionEvent(Collision collision)
        {
            LastDamageForce = collision.impulse.magnitude;
            LastRelativeVelocity = collision.relativeVelocity.magnitude;

            Debug.Log("Collision detected with object: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag + ", Force: " + LastDamageForce);

            if (LastDamageForce >= MinForce)
            {
                if (!collision.gameObject.CompareTag("Floor"))
                {
                    if (scoringSystem != null)
                    {
                        scoringSystem.AddScore(10); // Add 10 points
                        Debug.Log("Points added for destroying box.");
                    }
                    else
                    {
                        Debug.Log("Scoring system is null!");
                    }
                }
                else
                {
                    Debug.Log("Box destroyed by floor. No points added.");
                }

                Destroy(gameObject);
                Debug.Log("Box destroyed by: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);

                Damageable d = collision.gameObject.GetComponent<Damageable>();
                if (d)
                {
                    d.DealDamage(Damage, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject);
                }
                else if (TakeCollisionDamage && thisDamageable != null)
                {
                    thisDamageable.DealDamage(CollisionDamage, collision.GetContact(0).point, collision.GetContact(0).normal, true, gameObject, collision.gameObject);
                }
            }
            else
            {
                Debug.Log("Collision force too low to cause damage.");
            }
        }

    }
}
