using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform characterTransform; // Assign your character's transform here
    [SerializeField] private Vector3 cameraOffset; // The offset from the character
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minYAngle = -60f;
    [SerializeField] private float maxYAngle = 60f;

    private float currentYRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Optionally adjust the initial offset based on the current relative position
        //cameraOffset = transform.position - characterTransform.position;
    }

    private void LateUpdate()
    {
        // Move the camera to follow the character
        FollowCharacter();

        // Rotate the camera based on input
        RotateCamera();
    }

    private void FollowCharacter()
    {
        // Adjust the position based on the character's position and the fixed offset
        // Ensures the camera maintains a fixed distance behind the character
        Vector3 newPosition = characterTransform.position + characterTransform.TransformDirection(cameraOffset);
        transform.position = newPosition;
    }

    private void RotateCamera()
    {
        Vector2 lookDirection = PlayerInputs.Instance.lookInput;

        // Calculate new y rotation (x-axis rotation)
        currentYRotation += -lookDirection.y * rotationSpeed * Time.deltaTime;
        currentYRotation = Mathf.Clamp(currentYRotation, minYAngle, maxYAngle);

        // Apply the clamped rotation around the x-axis
        transform.localEulerAngles = new Vector3(currentYRotation, transform.localEulerAngles.y, 0);

        // Rotate around y-axis with horizontal input
        // Note: Consider rotating the character instead, and let the camera follow the character's rotation
        characterTransform.Rotate(Vector3.up, lookDirection.x * rotationSpeed * Time.deltaTime);
    }
}
