using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRig : MonoBehaviour
{
    public static WeaponRig Instance;

    [Header("Settings")]
    public int MaxWeaponsInInventory = 2;

    [Header("References")]
    private Weapon m_currentWeapon;
    public Weapon CurrentWeapon => m_currentWeapon;
    [Tooltip("This is the transform that is parent to the weapon gameobjects."),
        SerializeField]
    private Transform m_weaponHand;
    public Transform WeaponHand => m_weaponHand;
    public AudioSource AudioSource;

    [Header("Currently Unlocked Weapons List")]
    [SerializeField] private List<Weapon> m_unlockedWeapons;
    public List<Weapon> UnlockedWeapons => m_unlockedWeapons;
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
        PlayerInputs.Instance.OnSelect -= SelectWeapon;
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
            if (m_weaponHand.GetChild(i).TryGetComponent(out Weapon weapon))
            {
                m_unlockedWeapons.Add(weapon);
                weapon.gameObject.SetActive(false);
            }
        }

        if (m_unlockedWeapons.Count > 0)
        {
            m_currentWeapon = m_unlockedWeapons[0];
            WeaponChanged();
        }

        UpdateAmmoCounterMethod();
    }

    private void SelectWeapon(bool upOrDown)
    {
        if (m_unlockedWeapons.Count <= 0)
        {
            Debug.LogWarning("No weapons available to select.");
            return;
        }

        m_currentWeaponLocation = m_unlockedWeapons.IndexOf(m_currentWeapon);
        m_currentWeapon.gameObject.SetActive(false);


        if (upOrDown)
        {
            // Move to the next weapon, wrapping around to the start if at the end
            m_currentWeaponLocation++;
            if (m_currentWeaponLocation == m_unlockedWeapons.Count)
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
                m_currentWeaponLocation = m_unlockedWeapons.Count - 1;
            }
        }

        m_currentWeapon = m_unlockedWeapons[m_currentWeaponLocation];
        WeaponChanged();
    }

    private void WeaponChanged()
    {
        if (CurrentWeapon != null)
        {
            m_currentWeaponLocation = m_unlockedWeapons.IndexOf(m_currentWeapon);
            m_currentWeapon.gameObject.SetActive(true);
        }
        
        OnWeaponChanged?.Invoke();
        Invoke(nameof(UpdateAmmoCounterMethod), 0);
    }

    public void UpdateAmmoCounterMethod()
    {
        if (m_currentWeapon != null)
        {
            UpdateAmmoCounter?.Invoke(m_currentWeapon.CurrentAmmoInClip, m_currentWeapon.CurrentReserveAmmo);
        }
        else
        {
            ScreenspaceUIManager.Instance.ClearAmmoCounterText();
        }
    }

    public void TrySetWeapon(Weapon weapon)
    {
        weapon.enabled = true;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        if (UnlockedWeapons.Count == MaxWeaponsInInventory)
        {
            m_unlockedWeapons.Remove(CurrentWeapon);
            CurrentWeapon.transform.SetParent(null);
            CurrentWeapon.transform.SetPositionAndRotation(weapon.transform.position, weapon.transform.rotation);
            CurrentWeapon.enabled = false;

            if (CurrentWeapon.TryGetComponent(out CollectableWeapon collectableWeapon))
            {
                collectableWeapon.Collider.enabled = true;
            }
        }

        m_currentWeapon = weapon;

        m_currentWeapon.transform.SetParent(m_weaponHand);
        m_currentWeapon.transform.SetLocalPositionAndRotation(m_currentWeapon.PositionOffset, Quaternion.Euler(m_currentWeapon.RotationOffset));

        m_unlockedWeapons.Add(CurrentWeapon);

        WeaponChanged();
    }
}