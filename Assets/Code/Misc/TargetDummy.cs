using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [SerializeField] private HingeJoint m_hingeJoint;
    [SerializeField] private Damageable m_damageable;

    private void Start()
    {
        m_damageable.OnHit += MoveTargetDown;
    }

    [Button]
    public void MoveTargetDown(int _)
    {
        m_hingeJoint.useSpring = true;
    }

    [Button]
    public void MoveTargetUp()
    {
        m_hingeJoint.useSpring = false;
    }
}
