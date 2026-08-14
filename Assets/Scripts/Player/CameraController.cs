using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedParticleSystem;
    [SerializeField] private float zoomDuration = 0.3f;
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float maxFOV = 80f;
    [SerializeField] private float zoomSpeedModifier = 5f;

    private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void ChangeCameraFOV(float speedAmount)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine(speedAmount));

        if (speedParticleSystem != null)
        {
            if (speedAmount > 0)
                speedParticleSystem.Play();
            else
                speedParticleSystem.Stop();
        }
    }

    private IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView;

        float targetFOV = startFOV;

        if (speedAmount > 0)
            targetFOV = Mathf.Clamp(startFOV + zoomSpeedModifier, minFOV, maxFOV);
        else if (speedAmount < 0)
            targetFOV = Mathf.Clamp(startFOV - zoomSpeedModifier, minFOV, maxFOV);

        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;

            cinemachineCamera.Lens.FieldOfView =
                Mathf.Lerp(startFOV, targetFOV, elapsedTime / zoomDuration);

            yield return null;
        }

        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}