using Unity.Cinemachine;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private ParticleSystem collisionParticleSystem;
    [SerializeField] private float ShakeModifier = 10f;
    [SerializeField] AudioSource boulderSmashAudioSource;
    [SerializeField] float collisionCooldown = 1f;

    private CinemachineImpulseSource cinemachineImpulseSource;

    float collisionTimer = 1f;

    void Update()
    {
        collisionTimer += Time.deltaTime;
    }

     void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collisionTimer < collisionCooldown)
        {
            return;
        }
        FireImpulse();
        CollisionFX(collision);

        collisionTimer = 1f; // Pass the collision object
    }

     void FireImpulse()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        float shakeIntensity = (1f / distance) * ShakeModifier;
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);

        cinemachineImpulseSource.GenerateImpulse();
    }

     void CollisionFX(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        boulderSmashAudioSource.Play();
    }
}