using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance;

    [SerializeField] private Animator m_animator;

    [SerializeField] private RigBuilder m_rigBuilder;
    [SerializeField] private MultiAimConstraint m_bodyConstraint;
    [SerializeField] private MultiAimConstraint m_handAimConstraint;
    [SerializeField] private TwoBoneIKConstraint m_secondHandGrabWeapon;

    [SerializeField] private Transform m_secondHandGrabWeaponTarget;
    [SerializeField] private Transform m_secondHandGrabWeaponHint;
    [SerializeField] private Transform m_primaryHandGrabWeaponTarget;
    [SerializeField] private Transform m_primaryHandGrabWeaponHint;

    private Dictionary<WeaponType, float> weaponAnimationMap;

    private AnimationEventBroadcaster m_animationEventBroadcaster;

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
    }

    private void Start()
    {
        InitialiseWeaponTypeDictionary();
        InitialiseAnimationEventSubscriptions();
        PlayerInputs.Instance.OnSecondaryHeld += StartAiming;
        PlayerInputs.Instance.OnSecondaryReleased += StopAiming;
        WeaponRig.Instance.OnWeaponChanged += SwitchWeapon;

        if (WeaponRig.Instance.UnlockedWeapons.Count > 0)
        {
            Armed();
        }
        else
        {
            Unarmed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        float x = PlayerInputs.Instance.moveInput.x;
        float y = PlayerInputs.Instance.moveInput.y;

        m_animator.SetFloat("Horizontal", x, 0.1f, Time.deltaTime);
        m_animator.SetFloat("Vertical", y, 0.1f, Time.deltaTime);
    }

    [Button]
    private void LockAim()
    {
        PlayerInputs.Instance.OnSecondaryReleased -= StopAiming;
    }

    private void StartAiming()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeMultiAimConstraintWeightValueOverTime(m_bodyConstraint, 0.7f, 0.25f));
        StartCoroutine(ChangeMultiAimConstraintWeightValueOverTime(m_handAimConstraint, 1f, 0.05f));
        m_animator.SetBool("Aiming", true);
        if (WeaponRig.Instance.CurrentWeapon != null)
        {
            SetHandIK(
            WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponTargetAim,
            WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponHintAim,
            WeaponRig.Instance.CurrentWeapon.PrimaryHandGrabWeaponTargetAim,
            WeaponRig.Instance.CurrentWeapon.PrimaryHandGrabWeaponHintAim);
        }
    }

    private void StopAiming()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeMultiAimConstraintWeightValueOverTime(m_bodyConstraint, 0f, 0.25f));
        StartCoroutine(ChangeMultiAimConstraintWeightValueOverTime(m_handAimConstraint, 0f, 0.25f));
        m_animator.SetBool("Aiming", false);
        if (WeaponRig.Instance.CurrentWeapon != null)
        {
            SetHandIK(
            WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponTargetIdle,
            WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponHintIdle,
            WeaponRig.Instance.CurrentWeapon.PrimaryHandGrabWeaponTargetIdle,
            WeaponRig.Instance.CurrentWeapon.PrimaryHandGrabWeaponHintIdle);
        }
    }

    public void Reload()
    {
        m_animator.Play("ReloadWeapon", 2);
        m_secondHandGrabWeapon.weight = 0f;
    }

    private void ReloadComplete()
    {
        WeaponRig.Instance.UpdateAmmoCounterMethod();
        Invoke(nameof(SetSecondHandGrabWeightToMax), 0);
    }

    private void SetSecondHandGrabWeightToMax()
    {
        StartCoroutine(ChangeTwoBoneIKConstraintWeightValueOverTime(m_secondHandGrabWeapon, 1f, 0.25f));
    }

    private IEnumerator ChangeTwoBoneIKConstraintWeightValueOverTime(TwoBoneIKConstraint twoBoneIKConstraint, float targetVal, float duration)
    {
        float startVal = twoBoneIKConstraint.weight;
        float timeElapsed = 0f;

        // Recursive step function
        void Step()
        {
            if (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                twoBoneIKConstraint.weight = Mathf.Lerp(startVal, targetVal, timeElapsed / duration);
                StartCoroutine(StepCoroutine()); // Schedule next step
            }
            else
            {
                twoBoneIKConstraint.weight = targetVal; // Ensure target value is set at the end
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

    private IEnumerator ChangeMultiAimConstraintWeightValueOverTime(MultiAimConstraint multiAimConstraint, float targetVal, float duration)
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

    private void InitialiseAnimationEventSubscriptions()
    {
        AnimationEventBroadcaster[] behaviours = m_animator.GetBehaviours<AnimationEventBroadcaster>();
        if (behaviours.Length > 0)
        {
            m_animationEventBroadcaster = behaviours[0]; // assuming it's the first one
            m_animationEventBroadcaster.ReloadComplete += ReloadComplete;
        }
    }

    private void InitialiseWeaponTypeDictionary()
    {
        weaponAnimationMap = new Dictionary<WeaponType, float>()
        {
            // Start with 1 because 0 is unarmed in the animator, but not a "Weapon Type" so it doesn't need a value here
            { WeaponType.pistol, 1 },
            { WeaponType.assaultRifle, 2 },
            // Map new weapon types to their animation IDs here
        };
    }

    public void SwitchWeapon()
    {
        if (WeaponRig.Instance.CurrentWeapon == null)
        {
            m_animator.SetFloat("WeaponType", 0);
            Unarmed();
        }
        else
        {
            m_animator.SetFloat("WeaponType", weaponAnimationMap[WeaponRig.Instance.CurrentWeapon.WeaponType]);
            Armed();
            SetHandIK(
            WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponTargetIdle,
            WeaponRig.Instance.CurrentWeapon.SecondHandGrabWeaponHintIdle,
            WeaponRig.Instance.CurrentWeapon.PrimaryHandGrabWeaponTargetIdle,
            WeaponRig.Instance.CurrentWeapon.PrimaryHandGrabWeaponHintIdle);
        }
    }

    private void Unarmed()
    {
        m_animator.SetLayerWeight(m_animator.GetLayerIndex("Action Layer"), 0);
        m_animator.SetLayerWeight(m_animator.GetLayerIndex("Armed Locomotion Layer"), 0);
        m_secondHandGrabWeapon.weight = 0f;
    }

    private void Armed()
    {
        m_animator.SetLayerWeight(m_animator.GetLayerIndex("Action Layer"), 1);
        m_animator.SetLayerWeight(m_animator.GetLayerIndex("Armed Locomotion Layer"), 1);
        m_secondHandGrabWeapon.weight = 1.0f;
    }

    private void SetHandIK(Transform secondHandTarget, Transform secondHandHint, Transform primaryHandTarget, Transform primaryHandHint)
    {
        m_secondHandGrabWeaponTarget.localPosition = secondHandTarget.localPosition;
        m_secondHandGrabWeaponTarget.localRotation = secondHandTarget.localRotation;
        m_secondHandGrabWeaponHint.localPosition = secondHandHint.localPosition;
        m_secondHandGrabWeaponHint.localRotation = secondHandHint.localRotation;

        m_primaryHandGrabWeaponTarget.localPosition = primaryHandTarget.localPosition;
        m_primaryHandGrabWeaponTarget.localRotation = primaryHandTarget.localRotation;
        m_primaryHandGrabWeaponHint.localPosition = primaryHandHint.localPosition;
        m_primaryHandGrabWeaponHint.localRotation = primaryHandHint.localRotation;
    }
}