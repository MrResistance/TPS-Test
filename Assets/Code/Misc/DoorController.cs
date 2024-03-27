using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool m_openAtCheckpoint = true;

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
        if (m_openAtCheckpoint)
        {
            m_checkpointArea.OnCheckpointReached += Open;
        }
        else
        {
            m_checkpointArea.OnCheckpointReached += Close;
        }

        GameManager.Instance.OnResetMap += Reset;
    }

    private void UpdateRotation()
    {
        if (rotationFraction < 1.0f)
        {
            rotationFraction += rotationSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationFraction);
        }
        else
        {
            transform.localRotation = targetRotation; // Ensure exact rotation
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

    private void Reset()
    {
        Debug.Log(name + " resetting...");
        Close();
    }
}
