using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootForce = 20f;
    public float fireRate = 0.5f;
    private float nextFireTime;

    void Update()
    {
        // Check for "A" button input
        if (IsFireButtonHeld() && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    // Method to check if the fire button is held
    bool IsFireButtonHeld()
    {
        // Replace this with the appropriate input method for your VR setup
        return Input.GetKey(KeyCode.JoystickButton0); // Typically "A" button on most controllers
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);
    }
}
