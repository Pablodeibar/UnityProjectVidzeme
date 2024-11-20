using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootForce = 200f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    public AudioClip fireSound; 
    private AudioSource audioSource;

    private InputAction fireAction;

    public LineRenderer lineRenderer; // Add this line
    public float lineLength = 10f;    // Length of the line renderer

    void Awake()
    {
        fireAction = new InputAction(type: InputActionType.Button, binding: "<XRController>{RightHand}/trigger");
        fireAction.Enable();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Pistolet pr�t � tirer.");

        // Initialize LineRenderer if not assigned
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"))
            {
                color = Color.green
            };
        }
    }

    void Update()
    {
        Debug.Log($"Heure actuelle: {Time.time}, Prochaine heure de tir: {nextFireTime}");

        if (fireAction.triggered && Time.time >= nextFireTime)
        {
            Debug.Log("Bouton de tir press�.");
            Shoot();
            nextFireTime = Time.time + fireRate;
            Debug.Log($"Prochaine heure de tir mise � jour �: {nextFireTime}");
        }

        UpdateLineRenderer(); // Add this line to update the line renderer position
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);

        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        Debug.Log("Balle tir�e.");
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * lineLength);
    }

    void OnDestroy()
    {
        fireAction.Dispose();
    }
}
