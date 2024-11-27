using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourGameNamespace
{
    public class ItemLauncher : MonoBehaviour
    {
        public GameObject[] projectilePrefabs;  
        public Transform launchPoint;           
        public float launchForce = 20f;         
        public float launchRate = 1f;           
        public float launchAngle = 50f;         
        public LineRenderer trajectoryLine;     
        public int trajectoryResolution = 50;   
        public float timeStep = 0.1f;           

        void Start()
        {
            StartCoroutine(LaunchProjectiles());
        }

        IEnumerator LaunchProjectiles()
        {
            while (true)
            {
                LaunchProjectile();
                yield return new WaitForSeconds(launchRate);
            }
        }

        void LaunchProjectile()
        {
            int randomIndex = Random.Range(0, projectilePrefabs.Length);
            GameObject selectedPrefab = projectilePrefabs[randomIndex];

            GameObject projectile = Instantiate(selectedPrefab, launchPoint.position, launchPoint.rotation);

            float randomAngle = Random.Range(-launchAngle, launchAngle);
            Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
            Vector3 launchDirection = rotation * launchPoint.forward;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);

            UpdateTrajectoryLine(launchDirection, projectile);
        }

        void UpdateTrajectoryLine(Vector3 direction, GameObject projectile)
        {
            Vector3[] points = new Vector3[trajectoryResolution];

            Vector3 startPosition = launchPoint.position;
            Vector3 startVelocity = direction * launchForce / projectile.GetComponent<Rigidbody>().mass;

            for (int i = 0; i < trajectoryResolution; i++)
            {
                float t = i * timeStep;
                points[i] = CalculateTrajectoryPoint(startPosition, startVelocity, t);
            }

            trajectoryLine.positionCount = trajectoryResolution;
            trajectoryLine.SetPositions(points);
        }

        Vector3 CalculateTrajectoryPoint(Vector3 startPos, Vector3 startVel, float t)
        {
            return startPos + startVel * t + 0.5f * Physics.gravity * t * t;
        }
    }
}
