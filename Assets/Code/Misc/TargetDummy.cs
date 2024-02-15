using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [SerializeField] private HingeJoint m_hingeJoint;

    public event Action OnTargetShot;

    [Button]
    public void MoveTargetDown()
    {
        m_hingeJoint.useSpring = true;
    }

    [Button]
    public void MoveTargetUp()
    {
        m_hingeJoint.useSpring = false;
    }
}
