using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_characterTransform;
    [SerializeField] private Vector3 m_cameraOffset;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private float m_minYAngle = -60f;
    [SerializeField] private float m_maxYAngle = 60f;

    [SerializeField] private float m_ADS_FOV;
    [SerializeField] private float m_HipFire_FOV;

    private float m_transitionProgress;
    private float m_transitionDuration = 0.5f;
    private bool m_isAimingDownSight;
    
    private float currentYRotation = 0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (PlayerInputs.Instance == null)
        {
            return; 
        }
        PlayerInputs.Instance.OnSecondaryPressed -= AimDownSight;
        PlayerInputs.Instance.OnSecondaryReleased -= AimFromHip;
        PlayerInputs.Instance.OnSecondaryPressed += AimDownSight;
        PlayerInputs.Instance.OnSecondaryReleased += AimFromHip;
    }

    private void OnEnable()
    {
        if (PlayerInputs.Instance == null)
        {
            return;
        }
        PlayerInputs.Instance.OnSecondaryPressed -= AimDownSight;
        PlayerInputs.Instance.OnSecondaryReleased -= AimFromHip;
        PlayerInputs.Instance.OnSecondaryPressed += AimDownSight;
        PlayerInputs.Instance.OnSecondaryReleased += AimFromHip;
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.OnSecondaryPressed -= AimDownSight;
        PlayerInputs.Instance.OnSecondaryReleased -= AimFromHip;
    }
    private void OnDestroy()
    {
        PlayerInputs.Instance.OnSecondaryPressed -= AimDownSight;
        PlayerInputs.Instance.OnSecondaryReleased -= AimFromHip;
    }

    private void LateUpdate()
    {
        FollowCharacter();
        RotateCamera();
        TransitionView();
    }

    private void FollowCharacter()
    {
        Vector3 newPosition = m_characterTransform.position + m_characterTransform.TransformDirection(m_cameraOffset);
        transform.position = newPosition;
    }

    private void RotateCamera()
    {
        Vector2 lookDirection = PlayerInputs.Instance.lookInput;

        // Calculate new y rotation (x-axis rotation)
        currentYRotation += -lookDirection.y * m_rotationSpeed * Time.deltaTime;
        currentYRotation = Mathf.Clamp(currentYRotation, m_minYAngle, m_maxYAngle);

        // Apply the clamped rotation around the x-axis
        transform.localEulerAngles = new Vector3(currentYRotation, transform.localEulerAngles.y, 0);

        // Rotate around y-axis with horizontal input
        m_characterTransform.Rotate(Vector3.up, lookDirection.x * m_rotationSpeed * Time.deltaTime);
    }

    private void AimDownSight()
    {
        m_isAimingDownSight = true;
        m_transitionProgress = 0f; // Reset progress
    }

    private void AimFromHip()
    {
        m_isAimingDownSight = false;
        m_transitionProgress = 0f; // Reset progress
    }
    
    private void TransitionView()
    {
        if (m_transitionProgress < 1.0f)
        {
            m_transitionProgress += Time.deltaTime / m_transitionDuration;

            if (m_isAimingDownSight)
            {
                m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_ADS_FOV, m_transitionProgress);

                //if (m_currentWeapon.Type == Weapon.WeaponType.sniper)
                //{
                //    m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_ADS_Sniper_FOV, m_transitionProgress);
                //    //m_sniperScopeCamera.fieldOfView = Mathf.Lerp(m_sniperScopeCamera.fieldOfView, m_SniperScope_FOV, m_transitionProgress);
                //}
                //else
                //{
                    
                //}
            }
            else
            {
                m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_HipFire_FOV, m_transitionProgress);
            }
        }
    }
}
