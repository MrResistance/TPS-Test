using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public string Name;

    [Header("Settings")]
    public Weapon.FireMode FireMode;

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
