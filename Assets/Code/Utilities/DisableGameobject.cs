using UnityEngine;

public class DisableGameobject : MonoBehaviour
{
    [SerializeField] private float m_delay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DisableGameobjectMethod), m_delay);
    }

    private void DisableGameobjectMethod()
    {
        gameObject.SetActive(false);
    }
}
