using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteract()
    {
        ScreenspaceUIManager.Instance.ClearInteractText();
    }
}
