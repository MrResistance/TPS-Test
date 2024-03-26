using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public string Name;
    public string UsablePrefabFilePath;

    [Header("Settings")]
    public FireMode FireMode;
    public WeaponType WeaponType;
    public float HapticShotStrength;

    [Header("Stats")]
    public float HitForce;
    public int Damage;
    public int EffectiveRange;
    public float FireRateCooldown;
    public Vector2 RecoilDistance;
    public float RecoilStrength;
    public Vector2 Spread;
    public int ImpactsPerShot;

    [Header("Ammo")]
    public int MaxClipSize;
    public int MaxReserveAmmo;
    public int CurrentReserveAmmo;

    [Header("Audio")]
    public List<AudioClip> CockWeapon_SFX;
    public List<AudioClip> DryFire_SFX;
    public List<AudioClip> EjectMag_SFX;
    public List<AudioClip> InsertMag_SFX;
    public List<AudioClip> SafetySwitch_SFX;
    public List<AudioClip> Fire_SFX;
    public List<AudioClip> Slide_SFX;
}

public enum FireMode { semiAuto, fullAuto }
public enum WeaponType { pistol, assaultRifle, shotgun }
