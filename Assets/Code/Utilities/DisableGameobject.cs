using UnityEngine;

public class DisableGameobject : MonoBehaviour
{
    [SerializeField] private float m_delay = 1f;

    private void OnEnable()
    {
        Invoke(nameof(DisableGameobjectMethod), m_delay);
    }

    private void DisableGameobjectMethod()
    {
        gameObject.SetActive(false);
    }
}
