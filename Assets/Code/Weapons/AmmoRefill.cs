using UnityEngine;

public class AmmoRefill : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();
        for (int i = 0; i < WeaponRig.Instance.UnlockedWeapons.Count; i++)
        {
            WeaponRig.Instance.UnlockedWeapons[i].SetReserveAmmoToMax();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to refill ammo");
    }

    private void OnTriggerExit(Collider other)
    {
        ScreenspaceUIManager.Instance.ClearInteractText();
    }
}
