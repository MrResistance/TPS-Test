using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private CheckpointArea m_checkpointArea;

    [Header("Rotation")]
    [SerializeField] private Vector3 m_closedRotation;
    [SerializeField] private Vector3 m_openRotation;
    [SerializeField] private float duration = 1.0f;

    [Header("Audio")]
    [SerializeField] private AudioClip m_open_SFX;
    [SerializeField] private AudioClip m_close_SFX;
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
        if (m_AudioSource != null)
        {
            m_AudioSource.clip = m_open_SFX;
            m_AudioSource.Play();
        }
        targetRotation = Quaternion.Euler(m_openRotation);
        rotationFraction = 0.0f;
        rotationSpeed = 1.0f / duration;
        CancelInvoke(nameof(UpdateRotation)); 
        InvokeRepeating(nameof(UpdateRotation), 0f, Time.deltaTime);
    }

    private void Close()
    {
        if (m_AudioSource != null)
        {
            m_AudioSource.clip = m_close_SFX;
            m_AudioSource.Play();
        }
        targetRotation = Quaternion.Euler(m_closedRotation);
        rotationFraction = 0.0f;
        rotationSpeed = 1.0f / duration; 
        CancelInvoke(nameof(UpdateRotation)); 
        InvokeRepeating(nameof(UpdateRotation), 0f, Time.deltaTime);
    }
}
