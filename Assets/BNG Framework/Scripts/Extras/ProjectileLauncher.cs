using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace BNG
{
    public class ProjectileLauncher : MonoBehaviour
    {

        public GameObject ProjectileObject;
        public float ProjectileForce = 15f;
        public AudioClip LaunchSound;
        public ParticleSystem LaunchParticles;
        public Transform MuzzleTransform;
        private float _initialProjectileForce;

        // Cooldown between launches
        public float launchCooldown = 1f;
        private float lastLaunchTime;

        // Random angle range
        public float angleRange = 15f;

        void Start()
        {
            _initialProjectileForce = ProjectileForce;
            lastLaunchTime = -launchCooldown; // Allows launching immediately
        }

        void Update()
        {
            CheckOculusButton();
            CheckSlowTimeButton();
        }

        void CheckOculusButton()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

            foreach (var device in devices)
            {
                bool primaryButtonState = false;
                if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) && primaryButtonState)
                {
                    if (Time.time >= lastLaunchTime + launchCooldown)
                    {
                        ShootProjectile();
                        lastLaunchTime = Time.time;
                    }
                }
            }
        }

        void CheckSlowTimeButton()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

            foreach (var device in devices)
            {
                bool secondaryButtonState = false;
                if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonState) && secondaryButtonState)
                {
                    ToggleSlowTime();
                }
            }
        }

        void ToggleSlowTime()
        {
            if (isTimeSlowed)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = slowTimeScale;
            }
            isTimeSlowed = !isTimeSlowed;
        }

        public GameObject ShootProjectile(float projectileForce)
        {
            if (MuzzleTransform && ProjectileObject)
            {
                // Adjust MuzzleTransform rotation randomly
                Vector3 randomRotation = MuzzleTransform.rotation.eulerAngles;
                randomRotation.x += Random.Range(-angleRange, angleRange);
                randomRotation.y += Random.Range(-angleRange, angleRange);
                MuzzleTransform.rotation = Quaternion.Euler(randomRotation);

                GameObject launched = Instantiate(ProjectileObject, MuzzleTransform.transform.position, MuzzleTransform.transform.rotation);
                Rigidbody rb = launched.GetComponentInChildren<Rigidbody>();

                if (rb)
                {
                    rb.AddForce(MuzzleTransform.forward * projectileForce, ForceMode.VelocityChange);
                }
                else
                {
                    Debug.LogError("No Rigidbody found on projectile!");
                }

                if (LaunchSound)
                {
                    VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
                }

                if (LaunchParticles)
                {
                    LaunchParticles.Play();
                }

                return launched;
            }
            else
            {
                Debug.LogError("MuzzleTransform or ProjectileObject is not assigned!");
            }

            return null;
        }

        public void ShootProjectile()
        {
            ShootProjectile(ProjectileForce);
        }

        public void SetForce(float force)
        {
            ProjectileForce = force;
        }

        public float GetInitialProjectileForce()
        {
            return _initialProjectileForce;
        }

        // Variables for slowing time
        public float slowTimeScale = 0.5f;
        private bool isTimeSlowed = false;
    }
}
