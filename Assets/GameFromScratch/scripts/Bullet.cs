using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // Time in seconds before the bullet is destroyed automatically

    void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet on collision
        Destroy(gameObject);
    }

    void Update()
    {
        // Optional: Check if the bullet goes out of bounds
        if (transform.position.y < -10) // Example condition for out of bounds
        {
            Destroy(gameObject);
        }
    }
}
