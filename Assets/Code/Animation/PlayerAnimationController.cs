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
        m_animator.SetFloat("Horizontal", PlayerInputs.Instance.moveInput.x, 0.2f, Time.deltaTime);
        m_animator.SetFloat("Vertical", PlayerInputs.Instance.moveInput.y, 0.2f, Time.deltaTime);
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
