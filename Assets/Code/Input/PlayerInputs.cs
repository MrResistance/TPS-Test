using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public event Action OnPrimaryPressed;
    public event Action OnPrimaryHeld;
    public event Action OnPrimaryReleased;
    public event Action OnSecondaryPressed;
    public event Action OnSecondaryHeld;
    public event Action OnSecondaryReleased;
    public event Action OnReload;
    public event Action OnJump;
    public event Action<bool> OnSelect;

    public Vector2 moveInput;
    public Vector2 lookInput;

    public PlayerControls controls;

    public static PlayerInputs Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        controls = new PlayerControls();

        controls.Actions.Primary.performed += PrimaryPressed;
        controls.Actions.Primary.canceled += PrimaryReleased;
        controls.Actions.Secondary.performed += SecondaryPressed;
        controls.Actions.Secondary.canceled += SecondaryReleased;
        controls.Actions.Reload.performed += Reload;
        controls.Actions.Jump.performed += Jump;
        controls.Actions.Select.performed += Select;
    }

    [Header("Settings"), SerializeField] private float m_selectInputCooldown = 0.25f;
    private float m_lastTimeSelectInputReceieved;
    private void Select(InputAction.CallbackContext context)
    {
        if (Time.time >= m_lastTimeSelectInputReceieved + m_selectInputCooldown)
        {
            switch (context.ReadValue<float>())
            {
                case > 0:
                    OnSelect?.Invoke(true);
                    m_lastTimeSelectInputReceieved = Time.time;
                    break;
                case < 0:
                    OnSelect?.Invoke(false);
                    m_lastTimeSelectInputReceieved = Time.time;
                    break;
                default:
                    break;
            }
        }
    }
    private void PrimaryPressed(InputAction.CallbackContext context)
    {
        OnPrimaryPressed?.Invoke();
    }

    private void PrimaryReleased(InputAction.CallbackContext context)
    {
        OnPrimaryReleased?.Invoke();
    }
    private void SecondaryPressed(InputAction.CallbackContext context)
    {
        OnSecondaryPressed?.Invoke();
    }
    private void SecondaryReleased(InputAction.CallbackContext context)
    {
        OnSecondaryReleased?.Invoke();
    }

    private void Reload(InputAction.CallbackContext context)
    {
        OnReload?.Invoke();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    #region Enabling/Disabling Action Maps
    private void Start()
    {
        controls.Enable();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
    #endregion

    void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        moveInput = controls.Movement.Move.ReadValue<Vector2>();
        lookInput = controls.Movement.Look.ReadValue<Vector2>();

        if (controls.Actions.Primary.phase == InputActionPhase.Performed)
        {
            OnPrimaryHeld?.Invoke();
        }

        if (controls.Actions.Secondary.phase == InputActionPhase.Performed)
        {
            OnSecondaryHeld?.Invoke();
        }
    }
}
