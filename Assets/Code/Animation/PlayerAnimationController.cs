using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    [SerializeField] private MultiAimConstraint m_bodyConstraint;
    [SerializeField] private MultiAimConstraint m_handAimConstraint;
    [SerializeField] private TwoBoneIKConstraint m_secondHandGrabWeapon;

    private void Start()
    {
        PlayerInputs.Instance.OnSecondaryHeld += StartAiming;
        PlayerInputs.Instance.OnSecondaryReleased += StopAiming;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        #region Snap inputs
        float roundedX = PlayerInputs.Instance.moveInput.x;
        float roundedY = PlayerInputs.Instance.moveInput.y;
        
        if (roundedX < 0 && roundedX >= -0.5f)
        {
            roundedX = -0.5f;
        }
        else if (roundedX < -0.5f)
        {
            roundedX = -1;
        }
        else if (roundedX > 0 && roundedX <= 0.5)
        {
            roundedX = 0.5f;
        }
        else if (roundedX > 0.5)
        {
            roundedX = 1;
        }

        if (roundedY < 0 && roundedY >= -0.5f)
        {
            roundedY = -0.5f;
        }
        else if (roundedY < -0.5f)
        {
            roundedY = -1;
        }
        else if (roundedY > 0 && roundedY <= 0.5)
        {
            roundedY = 0.5f;
        }
        else if (roundedY > 0.5)
        {
            roundedY = 1;
        }

        if (roundedX < 0.05 && roundedX > -0.05)
        {
            roundedX = 0;
        }
        if (roundedY < 0.05 && roundedY > -0.05)
        {
            roundedY = 0;
        }
        #endregion

        m_animator.SetFloat("Horizontal", roundedX, 0.1f, Time.deltaTime);
        m_animator.SetFloat("Vertical", roundedY, 0.1f, Time.deltaTime);
    }

    private void StartAiming()
    {
        m_bodyConstraint.weight = 0.7f;
        m_handAimConstraint.weight = 1f;
        m_animator.SetBool("Aiming", true);
    }

    private void StopAiming()
    {
        m_bodyConstraint.weight = 0f;
        m_handAimConstraint.weight = 0f;
        m_animator.SetBool("Aiming", false);
    }
}
