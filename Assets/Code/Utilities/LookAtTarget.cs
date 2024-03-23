using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private bool m_LookAtPlayerCamera = true;
    [SerializeField] private Transform m_Target;
    // Start is called before the first frame update
    void Start()
    {
        if (m_LookAtPlayerCamera)
        {
            m_Target = CameraController.Instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target != null) 
        {
            transform.LookAt(m_Target);
            transform.Rotate(0, 180f, 0);
        }
    }
}
