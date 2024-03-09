using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float shakeIntensity = 0.2f; // Intensity of the camera shake
    public float shakeSpeed = 10f; // Speed of the camera shake
    public float shakeFrequency = 1f; // Frequency of the camera shake
    public float shakeDistance = 0.1f; // Maximum distance of the camera shake

    private Vector3 originalPosition;

    private void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        // Save the original position of the camera
        originalPosition = virtualCamera.transform.localPosition;
    }

    public void MoveCamera(float strength)
    {
        // Calculate random noise for the camera shake
        float shakeAmount = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * shakeIntensity * shakeDistance;
        Vector3 shakeOffset = new Vector3(Mathf.PerlinNoise(0, Time.time * shakeSpeed) - 0.5f,
                                          Mathf.PerlinNoise(Time.time * shakeSpeed, 0) - 0.5f,
                                          Mathf.PerlinNoise(Time.time * shakeSpeed, Time.time * shakeSpeed) - 0.5f) * shakeAmount * strength;

        // Apply the shake to the camera's position
        virtualCamera.transform.localPosition = originalPosition + shakeOffset;
    }

    public void Reset()
    {
        // Reset camera position when not moving
        virtualCamera.transform.localPosition = originalPosition;
    }
}
