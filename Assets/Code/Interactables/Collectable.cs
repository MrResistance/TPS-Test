using UnityEngine;

public class Collectable : Interactable
{
    private void OnTriggerEnter(Collider other)
    {
        ScreenspaceUIManager.Instance.UpdateInteractText("Press <color=yellow><b>F</b></color> to collect");
    }

    private void OnTriggerExit(Collider other) 
    {
        ScreenspaceUIManager.Instance.ClearInteractText();
    }
}
