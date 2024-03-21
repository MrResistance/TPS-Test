using Sirenix.OdinInspector;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.7f;
    public float rotateMagnitude = 0.1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float currentShakeDuration;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            // Create a random rotation that respects the rotateMagnitude
            Quaternion randomRotation = new Quaternion(
                Random.Range(-rotateMagnitude, rotateMagnitude),
                Random.Range(-rotateMagnitude, rotateMagnitude),
                Random.Range(-rotateMagnitude, rotateMagnitude),
                1).normalized;

            // Apply this rotation to the initial rotation
            transform.localRotation = initialRotation * randomRotation;

            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.localPosition = initialPosition;
            transform.localRotation = initialRotation;
        }
    }

    [Button]
    public void TriggerShake(float duration, float magnitude, float rotationMagnitude)
    {
        rotateMagnitude = rotationMagnitude;
        shakeMagnitude = magnitude;
        currentShakeDuration = duration;
    }
}
