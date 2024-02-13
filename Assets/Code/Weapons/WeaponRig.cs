using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRig : MonoBehaviour
{
    public static WeaponRig Instance;

    [Header("References")]
    [SerializeField] private Weapon m_currentWeapon;
    [Tooltip("This is the transform that is parent to the weapon gameobjects."),
        SerializeField] private Transform m_weaponHand;
    public Weapon CurrentWeapon => m_currentWeapon;
    public AudioSource AudioSource;

    [Header("Current Weapons List")]
    [SerializeField] private List<Weapon> m_weapons;
    [SerializeField] private int m_currentWeaponLocation;

    public event Action<int, int> UpdateAmmoCounter;
    public event Action OnWeaponChanged;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    #region Event Subscriptions
    private void Start()
    {
        PlayerInputs.Instance.OnSelect += SelectWeapon;
        InitialiseWeapons();
    }
    private void OnEnable()
    {
        if (PlayerInputs.Instance == null) return;
        PlayerInputs.Instance.OnSelect -= SelectWeapon;
        PlayerInputs.Instance.OnSelect += SelectWeapon;
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.OnSelect -= SelectWeapon;
    }

    private void OnDestroy()
    {
        PlayerInputs.Instance.OnSelect -= SelectWeapon;
    }
    #endregion

    private void InitialiseWeapons()
    {
        for (int i = 0; i < m_weaponHand.childCount; i++)
        {
            if (m_weaponHand.GetChild(i).TryGetComponent(out Weapon weapon) && weapon.WeaponUnlocked)
            {
                m_weapons.Add(weapon);
                weapon.gameObject.SetActive(false);
            }
        }

        if (m_weapons.Count > 0)
        {
            m_currentWeapon = m_weapons[0];
            CurrentWeaponSetup();
        }
    }

    private void SelectWeapon(bool upOrDown)
    {
        if (m_weapons.Count <= 0)
        {
            Debug.LogWarning("No weapons available to select.");
            return;
        }

        m_currentWeaponLocation = m_weapons.IndexOf(m_currentWeapon);
        m_currentWeapon.gameObject.SetActive(false);

        if (upOrDown)
        {
            // Move to the next weapon, wrapping around to the start if at the end
            m_currentWeaponLocation++;
            if (m_currentWeaponLocation == m_weapons.Count)
            {
                m_currentWeaponLocation = 0;
            }
        }
        else
        {
            // Move to the previous weapon, wrapping around to the end if at the start
            m_currentWeaponLocation--;
            if (m_currentWeaponLocation < 0)
            {
                m_currentWeaponLocation = m_weapons.Count - 1;
            }
        }

        m_currentWeapon = m_weapons[m_currentWeaponLocation];
        CurrentWeaponSetup();
    }

    private void CurrentWeaponSetup()
    {
        m_currentWeapon.gameObject.SetActive(true);
        UpdateAmmoCounterMethod();
        OnWeaponChanged?.Invoke();
    }

    public void UpdateAmmoCounterMethod()
    {
        if (m_currentWeapon != null)
        {
            UpdateAmmoCounter?.Invoke(m_currentWeapon.CurrentAmmoInClip, m_currentWeapon.CurrentReserveAmmo);
        }
    }
}