using UnityEngine;

public class CollectableWeapon : Collectable
{
    [SerializeField] private WeaponData m_weaponData;

    private void OnTriggerEnter(Collider other)
    {
        ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to swap " + WeaponRig.Instance.CurrentWeapon.name + " for " + m_weaponData.Name);
        PlayerInputs.Instance.OnInteractPressed += CollectWeapon;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving " + m_weaponData.Name + " range.");
        ScreenspaceUIManager.Instance.ClearInteractText();
        PlayerInputs.Instance.OnInteractPressed -= CollectWeapon;
    }

    private void CollectWeapon()
    {
        Debug.Log("Attempting to collect " + m_weaponData.Name + "...");
        if (WeaponRig.Instance.CurrentWeapon.name != m_weaponData.name)
        {
            WeaponRig.Instance.SetWeapon(m_weaponData);
            ScreenspaceUIManager.Instance.ClearInteractText();
            DestroyWeapon();
        }
    }

    private void DestroyWeapon()
    {
        PlayerInputs.Instance.OnInteractPressed -= CollectWeapon;
        Destroy(gameObject);
    }
}
