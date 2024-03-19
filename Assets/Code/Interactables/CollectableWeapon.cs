using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class CollectableWeapon : Collectable
{
    [SerializeField] private Weapon m_weapon;
    public Collider Collider;

    public override void OnTriggerEnter(Collider other)
    {
        if (WeaponRig.Instance.CurrentWeapon != null && WeaponRig.Instance.UnlockedWeapons.Count == WeaponRig.Instance.MaxWeaponsInInventory)
        {
            ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to swap " + WeaponRig.Instance.CurrentWeapon.name + " for " + m_weapon.WeaponData.Name);
        }
        else if (WeaponRig.Instance.UnlockedWeapons.Count != WeaponRig.Instance.MaxWeaponsInInventory)
        {
            ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to pick up " + m_weapon.WeaponData.Name);
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();

        Collider.enabled = false;

        WeaponRig.Instance.TrySetWeapon(m_weapon);
    }
}
