using UnityEngine;
public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minYAngle = -60f;
    [SerializeField] private float maxYAngle = 60f;

    private float currentYRotation = 0f;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        RotateCamera();
    }
    private void RotateCamera()
    {
        Vector2 lookDirection = PlayerInputs.Instance.lookInput;
        // Rotate around y-axis with horizontal input
        transform.Rotate(Vector3.up, lookDirection.x * rotationSpeed * Time.deltaTime, Space.World);

        // Calculate new y rotation (x-axis rotation)
        currentYRotation += -lookDirection.y * rotationSpeed * Time.deltaTime;
        currentYRotation = Mathf.Clamp(currentYRotation, minYAngle, maxYAngle);

        // Apply the clamped rotation around the x-axis
        transform.localEulerAngles = new Vector3(currentYRotation, transform.localEulerAngles.y, 0);
    }
}