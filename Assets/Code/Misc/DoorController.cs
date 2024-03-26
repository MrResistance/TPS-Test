using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private CheckpointArea m_checkpointArea;
    [SerializeField] private Vector3 m_closedRotation;
    [SerializeField] private Vector3 m_openRotation;
    [SerializeField] private float duration = 1.0f;

    private Quaternion targetRotation;
    private float rotationSpeed;
    private float rotationFraction = 0.0f;

    void Start()
    {
        m_checkpointArea.OnCheckpointReached += Open;
    }

    private void UpdateRotation()
    {
        if (rotationFraction < 1.0f)
        {
            rotationFraction += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationFraction);
        }
        else
        {
            transform.rotation = targetRotation; // Ensure exact rotation
            CancelInvoke(nameof(UpdateRotation));
        }
    }

    private void Open()
    {
        targetRotation = Quaternion.Euler(m_openRotation);
        rotationFraction = 0.0f;
        rotationSpeed = 1.0f / duration;
        CancelInvoke(nameof(UpdateRotation)); 
        InvokeRepeating(nameof(UpdateRotation), 0f, Time.deltaTime);
    }

    private void Close()
    {
        targetRotation = Quaternion.Euler(m_closedRotation);
        rotationFraction = 0.0f;
        rotationSpeed = 1.0f / duration; 
        CancelInvoke(nameof(UpdateRotation)); 
        InvokeRepeating(nameof(UpdateRotation), 0f, Time.deltaTime);
    }
}
