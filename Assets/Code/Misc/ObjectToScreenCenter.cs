using UnityEngine;

public class ObjectToScreenCenter : MonoBehaviour
{
    public float m_distanceFromCamera = 10.0f; // Distance from the camera to the object
    protected Vector3 m_screenCenter;

    public virtual void Update()
    {
        // Calculate the screen center point
        m_screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, m_distanceFromCamera);

        // Convert the screen point to a world point
        transform.position = Camera.main.ScreenToWorldPoint(m_screenCenter);
    }
}
