using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float launchInterval = 1f;
    private float nextLaunchTime;

    void Update()
    {
        if (Time.time >= nextLaunchTime)
        {
            LaunchProjectile();
            nextLaunchTime = Time.time + launchInterval;
        }
    }

    void LaunchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }
}
