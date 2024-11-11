using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to be launched
    public Transform launchPoint;        // The point from which the projectile will be launched
    public float launchForce = 20f;      // The force with which the projectile will be launched
    public float launchRate = 1f;        // Time between launches in seconds

    void Start()
    {
        // Start the coroutine to launch projectiles automatically
        StartCoroutine(LaunchProjectiles());
    }

    IEnumerator LaunchProjectiles()
    {
        while (true)
        {
            LaunchProjectile();  // Launch a projectile
            yield return new WaitForSeconds(launchRate);  // Wait for the next launch
        }
    }

    void LaunchProjectile()
    {
        // Instantiate the projectile at the launch point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);
        // Apply force to the projectile's Rigidbody to launch it
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(launchPoint.forward * launchForce, ForceMode.Impulse);
    }
}
