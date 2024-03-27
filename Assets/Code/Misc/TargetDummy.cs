using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [SerializeField] private HingeJoint m_hingeJoint;
    [SerializeField] private Damageable m_damageable;
    [SerializeField] private CheckpointArea m_checkpointArea;
    private void Start()
    {
        m_damageable.OnHit += MoveTargetDown;

        GameManager.Instance.OnResetMap += Reset;

        if (m_checkpointArea != null)
        {
            m_checkpointArea.OnCheckpointReached += MoveTargetUp;
        }
    }

    public void MoveTargetDown(int _)
    {
        m_hingeJoint.useSpring = true;
    }

    public void MoveTargetUp()
    {
        m_hingeJoint.useSpring = false;
    }

    private void Reset()
    {
        MoveTargetDown(0);
    }
}
