using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData m_weaponData;
    public WeaponData WeaponData => m_weaponData;

    [Header("Settings")]
    public FireMode WeaponFireMode;
    public WeaponType WeaponType;

    //Stats
    public float m_hitForce;
    public int m_damage;
    public int m_effectiveRange;
    private float m_fireRateCooldown;
    private float m_hapticShotStrength;

    //Ammo
    private int m_maxClipSize;
    private int m_currentAmmoInClip;
    private int m_maxReserveAmmo;
    private int m_currentReserveAmmo;

    //Public accessors
    public int MaxClipSize => m_maxClipSize;
    public int CurrentAmmoInClip => m_currentAmmoInClip;
    public int CurrentReserveAmmo => m_currentReserveAmmo;

    public RaycastHit m_raycastHit;
    private int m_amountToReload;

    private float m_lastTimeFired;
    protected bool m_currentlyShooting = false;

    [Header("References")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private ParticleSystem m_gunshotFX;
    public Transform Barrel;

    [Header("IK Targeting - Idle")]
    public Transform SecondHandGrabWeaponTargetIdle;
    public Transform SecondHandGrabWeaponHintIdle;

    public Transform PrimaryHandGrabWeaponTargetIdle;
    public Transform PrimaryHandGrabWeaponHintIdle;

    [Header("IK Targeting - Aiming")]
    public Transform SecondHandGrabWeaponTargetAim;
    public Transform SecondHandGrabWeaponHintAim;

    public Transform PrimaryHandGrabWeaponTargetAim;
    public Transform PrimaryHandGrabWeaponHintAim;

    public event Action OnShoot;
    private AnimationEventBroadcaster m_animationEventBroadcaster;

    #region Event Subscriptions
    protected void Start()
    {
        InitialiseWeapon(true);
        InitialiseEventSubscriptions();
    }
    protected void OnEnable()
    {
        InitialiseWeapon(false);
        InitialiseEventSubscriptions();
    }

    private void InitialiseWeapon(bool start)
    {
        if (PlayerInputs.Instance == null)
        {
            return;
        }

        if (m_weaponData != null)
        {
            GetWeaponData(start);
        }
        else
        {
            Debug.LogWarning(name + " WeaponData not assigned. Weapon Data may be abnormal.");
        }
    }

    private void InitialiseEventSubscriptions()
    {
        if (PlayerInputs.Instance == null)
        {
            return;
        }

        switch (WeaponFireMode)
        {
            case FireMode.semiAuto:
                ReceivePrimaryPressedEvents();
                break;
            case FireMode.fullAuto:
                ReceivePrimaryHeldEvents();
                break;
        }
        PlayerInputs.Instance.OnReload -= Reload;
        PlayerInputs.Instance.OnReload += Reload;
        AnimationEventBroadcaster[] behaviours = m_animator.GetBehaviours<AnimationEventBroadcaster>();
        if (behaviours.Length > 0)
        {
            m_animationEventBroadcaster = behaviours[0]; // assuming it's the first one
            m_animationEventBroadcaster.ReloadComplete -= ReloadComplete;
            m_animationEventBroadcaster.ReloadComplete += ReloadComplete;
        }
    }

    protected void OnDisable()
    {
        StopReceivingPrimaryPressedEvents();
        StopReceivingPrimaryHeldEvents();
        PlayerInputs.Instance.OnReload -= Reload;
        m_animationEventBroadcaster.ReloadComplete -= Reload;
    }

    protected void OnDestroy()
    {
        StopReceivingPrimaryPressedEvents();
        StopReceivingPrimaryHeldEvents();
        PlayerInputs.Instance.OnReload -= Reload;
        m_animationEventBroadcaster.ReloadComplete -= Reload;
    }

    public void ReceivePrimaryPressedEvents()
    {
        PlayerInputs.Instance.OnPrimaryPressed -= RequestShot;
        PlayerInputs.Instance.OnPrimaryPressed += RequestShot;
    }

    public void ReceivePrimaryHeldEvents()
    {
        PlayerInputs.Instance.OnPrimaryHeld -= RequestShot;
        PlayerInputs.Instance.OnPrimaryHeld += RequestShot;
        PlayerInputs.Instance.OnPrimaryReleased -= StopShooting;
        PlayerInputs.Instance.OnPrimaryReleased += StopShooting;
    }

    public void StopReceivingPrimaryPressedEvents()
    {
        PlayerInputs.Instance.OnPrimaryPressed -= RequestShot;
    }

    public void StopReceivingPrimaryHeldEvents()
    {
        PlayerInputs.Instance.OnPrimaryHeld -= RequestShot;
        PlayerInputs.Instance.OnPrimaryReleased -= StopShooting;
    }
    #endregion

    #region Reloading
    public void Reload()
    {
        int reloadRequestResult = RequestReload();
        if (reloadRequestResult > 0)
        {
            if (GameSettings.Instance.RealisticReloadingAmmoCount)
            {
                LoseReserveAmmo(reloadRequestResult);
            }
            else
            {
                LoseReserveAmmo(reloadRequestResult - m_currentAmmoInClip);
            }

            //PlayRandomSFX(m_ejectMag);

            m_currentAmmoInClip = 0;
            m_amountToReload = reloadRequestResult;
            m_animator.SetTrigger("Reload");
            PlayerAnimationController.Instance.Reload();
        }
        else
        {
            //OnOutOfAmmoReserve?.Invoke();
        }
    }

    protected int RequestReload()
    {
        if (m_currentReserveAmmo == 0)
        {
            return 0; //If the player has no reserve ammo, they can't reload
        }
        if (m_currentAmmoInClip == m_maxClipSize)
        {
            return 0; //If the weapon is already fully loaded, they can't reload
        }
        if (m_currentReserveAmmo > 0 && m_currentReserveAmmo < m_maxClipSize)
        {
            return m_currentReserveAmmo;
            //If the player has reserve ammo, but the reserve ammo is less
            //than the max clip size then return whatever's there in reserve
        }
        if (m_currentReserveAmmo >= m_maxClipSize)
        {
            return m_maxClipSize;
            //If the player has more than or equal to the max clip size, return the max
        }
        return 0;
    }

    public virtual void ReloadComplete()
    {
        m_currentAmmoInClip += m_amountToReload;
        m_amountToReload = 0;

        //PlayRandomSFX(m_insertMag);
        WeaponRig.Instance.UpdateAmmoCounterMethod();
    }

    public void GainReserveAmmo(int amount)
    {
        m_currentReserveAmmo += amount;
    }

    public void LoseReserveAmmo(int amount)
    {
        m_currentReserveAmmo -= amount;
    }

    #endregion

    #region Shooting
    protected void RequestShot()
    {
        if (Time.time >= m_lastTimeFired + m_fireRateCooldown && !m_currentlyShooting)
        {
            m_lastTimeFired = Time.time;
            if (m_currentAmmoInClip > 0)
            {
                m_currentlyShooting = true;
                Shoot();
            }
            else
            {
                //PlayRandomSFX(m_dryFire);
            }
        }
    }

    public virtual void Shoot()
    {
        if (m_currentlyShooting)
        {
            m_currentAmmoInClip--;
            m_gunshotFX.Play();

            Invoke(nameof(StopShooting), m_fireRateCooldown);
            OnShoot?.Invoke();

            WeaponRig.Instance.UpdateAmmoCounterMethod();
            PlayerInputs.Instance.StartHapticFeedback(m_hapticShotStrength, m_fireRateCooldown / 2);

            //PlayRandomSFX(m_shot);
        }
    }
    
    protected void StopShooting()
    {
        PlayerInputs.Instance.StopHapticFeedback();
        m_currentlyShooting = false;
        m_gunshotFX.Stop();
    }
    #endregion

    private void PlayRandomSFX(List<AudioClip> sfx)
    {
        if (sfx.Count > 0)
        {
            //m_audioSource.PlayOneShot(sfx[UnityEngine.Random.Range(0, sfx.Count)]);
        }
    }

    protected void GetWeaponData(bool start)
    {
        gameObject.name = m_weaponData.Name;
        WeaponFireMode = m_weaponData.FireMode;
        WeaponType = m_weaponData.WeaponType;
        m_hapticShotStrength = m_weaponData.HapticShotStrength;
        m_hitForce = m_weaponData.HitForce;
        m_damage = m_weaponData.Damage;
        m_effectiveRange = m_weaponData.EffectiveRange;
        m_fireRateCooldown = m_weaponData.FireRateCooldown;
        m_maxClipSize = m_weaponData.MaxClipSize;
        m_maxReserveAmmo = m_weaponData.MaxReserveAmmo;

        if (start)
        {
            m_currentAmmoInClip = m_weaponData.MaxClipSize;
            m_currentReserveAmmo = m_weaponData.CurrentReserveAmmo;
        }
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }
}
