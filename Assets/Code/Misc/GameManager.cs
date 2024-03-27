using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action OnResetMap;

    [SerializeField] private CheckpointArea m_resetMapCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_resetMapCheckpoint.OnCheckpointReached += ResetMap;
    }

    private void ResetMap()
    {
        OnResetMap?.Invoke();
    }
}
