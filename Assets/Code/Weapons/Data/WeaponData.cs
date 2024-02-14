using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public string Name;

    [Header("Settings")]
    public FireMode FireMode;
    public WeaponType WeaponType;
    public float HapticShotStrength;

    [Header("Stats")]
    public float HitForce;
    public int Damage;
    public int EffectiveRange;
    public float FireRateCooldown;

    [Header("Ammo")]
    public int MaxClipSize;
    public int MaxReserveAmmo;
    public int CurrentReserveAmmo;
}

public enum FireMode { semiAuto, fullAuto }
public enum WeaponType { pistol, assaultRifle }
