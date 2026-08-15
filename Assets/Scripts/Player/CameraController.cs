using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedParticleSystem;

    [Header("FOV Settings")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float speedBoostFOV = 70f;
    [SerializeField] private float zoomDuration = 0.3f;

    private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();

        if (cinemachineCamera == null)
        {
            Debug.LogError("CinemachineCamera not found!");
            return;
        }

        cinemachineCamera.Lens.FieldOfView = normalFOV;
    }

    public void ChangeCameraFOV(float speedAmount)
    {
        StopAllCoroutines();

        if (speedAmount > 0)
        {
            StartCoroutine(ChangeFOVRoutine(speedBoostFOV));

            if (speedParticleSystem != null)
                speedParticleSystem.Play();
        }
        else if (speedAmount < 0)
        {
            StartCoroutine(ChangeFOVRoutine(normalFOV));

            if (speedParticleSystem != null)
                speedParticleSystem.Stop();
        }
    }

    private IEnumerator ChangeFOVRoutine(float targetFOV)
    {
        if (cinemachineCamera == null)
            yield break;

        float startFOV = cinemachineCamera.Lens.FieldOfView;

        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;

            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(
                startFOV,
                targetFOV,
                elapsedTime / zoomDuration
            );

            yield return null;
        }

        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}