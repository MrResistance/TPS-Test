public interface IInteractable
{
    public void OnInteract()
    {
        ScreenspaceUIManager.Instance.ClearInteractText();
    }
}
