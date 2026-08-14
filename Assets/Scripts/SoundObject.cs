using Unity.Cinemachine;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField] private float shakeModifier = 10f;
    [SerializeField] private AudioSource boulderSmashAudioSource;

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        FireImpulse();
        PlayCollisionSound();
    }

    private void FireImpulse()
    {
        if (Camera.main == null) return;

        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntensity = Mathf.Min((1f / Mathf.Max(distance, 0.1f)) * shakeModifier, 1f);

        // Generate camera shake
        cinemachineImpulseSource.GenerateImpulse();
    }

    private void PlayCollisionSound()
    {
        if (boulderSmashAudioSource != null)
        {
            boulderSmashAudioSource.Stop();
            boulderSmashAudioSource.Play();
        }
    }
}