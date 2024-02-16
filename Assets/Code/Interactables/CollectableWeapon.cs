using UnityEngine;

public class CollectableWeapon : Collectable
{
    [SerializeField] private WeaponData m_weaponData;

    private void OnTriggerEnter(Collider other)
    {
        if (WeaponRig.Instance.CurrentWeapon != null)
        {
            ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to swap " + WeaponRig.Instance.CurrentWeapon.name + " for " + m_weaponData.Name);
        }
        else
        {
            ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to pick up " + m_weaponData.Name);
        }
        PlayerInputs.Instance.OnInteractPressed += CollectWeapon;
    }

    private void OnTriggerExit(Collider other)
    {
        ScreenspaceUIManager.Instance.ClearInteractText();
        PlayerInputs.Instance.OnInteractPressed -= CollectWeapon;
    }

    private void CollectWeapon()
    {
        if (WeaponRig.Instance.CurrentWeapon != null)
        {
            SpawnReplacementWeaponCollectable();
        }

        WeaponRig.Instance.SetWeapon(m_weaponData);
        ScreenspaceUIManager.Instance.ClearInteractText();
        DestroyWeapon();
    }

    private void DestroyWeapon()
    {
        PlayerInputs.Instance.OnInteractPressed -= CollectWeapon;
        Destroy(gameObject);
    }

    private void SpawnReplacementWeaponCollectable()
    {
        string prefabPath = WeaponRig.Instance.CurrentWeapon.WeaponData.CollectablePrefabFilePath;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab != null)
        {
            Instantiate(prefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogError("Prefab not found at path: " + prefabPath);
        }
    }
}
