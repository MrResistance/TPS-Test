using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    [SerializeField] private MultiAimConstraint m_bodyConstraint;
    [SerializeField] private MultiAimConstraint m_handAimConstraint;
    [SerializeField] private TwoBoneIKConstraint m_secondHandGrabWeapon;

    [SerializeField] private Transform m_secondHandGrabWeaponTarget;
    [SerializeField] private Transform m_secondHandGrabWeaponHint;

    private Dictionary<WeaponType, float> weaponAnimationMap;

    private void Start()
    {
        PlayerInputs.Instance.OnSecondaryHeld += StartAiming;
        PlayerInputs.Instance.OnSecondaryReleased += StopAiming;
        InitialiseWeaponTypeDictionary();
        WeaponRig.Instance.OnWeaponChanged += SwitchWeapon;
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
        StopAllCoroutines();
        StartCoroutine(ChangeRigWeightValueOverTime(m_bodyConstraint, 0.7f, 0.25f));
        StartCoroutine(ChangeRigWeightValueOverTime(m_handAimConstraint, 1f, 0.05f));
        m_animator.SetBool("Aiming", true);
    }

    private void StopAiming()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeRigWeightValueOverTime(m_bodyConstraint, 0f, 0.25f));
        StartCoroutine(ChangeRigWeightValueOverTime(m_handAimConstraint, 0f, 0.25f));
        m_animator.SetBool("Aiming", false);
    }

    private IEnumerator ChangeRigWeightValueOverTime(MultiAimConstraint multiAimConstraint, float targetVal, float duration)
    {
        float startVal = multiAimConstraint.weight;
        float timeElapsed = 0f;

        // Recursive step function
        void Step()
        {
            if (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                multiAimConstraint.weight = Mathf.Lerp(startVal, targetVal, timeElapsed / duration);
                StartCoroutine(StepCoroutine()); // Schedule next step
            }
            else
            {
                multiAimConstraint.weight = targetVal; // Ensure target value is set at the end
            }
        }

        // Wrapper coroutine to call the step function
        IEnumerator StepCoroutine()
        {
            yield return null; // Wait for the next frame
            Step();
        }

        Step(); // Start the process
        yield return null; // This coroutine waits for the first call to Step() to finish
    }

    private void InitialiseWeaponTypeDictionary()
    {
        weaponAnimationMap = new Dictionary<WeaponType, float>()
        {
            { WeaponType.pistol, 0 },
            { WeaponType.assaultRifle, 1 },
            // Map new weapon types to their animation IDs here
        };
    }

    public void SwitchWeapon()
    {
        m_secondHandGrabWeaponTarget.localPosition = WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponTarget;
        m_secondHandGrabWeaponTarget.localRotation = WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponTargetRotation;
        m_secondHandGrabWeaponHint.localPosition = WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponHint;
        m_secondHandGrabWeaponHint.localRotation = WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponHintRotation;
        m_animator.SetFloat("WeaponType", weaponAnimationMap[WeaponRig.Instance.CurrentWeapon.WeaponType]);
    }
}
