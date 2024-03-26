using System;
using UnityEngine;

public class CheckpointArea : MonoBehaviour
{
    public event Action OnCheckpointReached;

    [SerializeField] private bool m_checkpointReached;
    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & GameSettings.Instance.PlayerLayer.value) != 0 && !m_checkpointReached)
        {
            m_checkpointReached = true;
            OnCheckpointReached?.Invoke();
        }
    }
}
