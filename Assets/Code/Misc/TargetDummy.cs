using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [SerializeField] private HingeJoint m_hingeJoint;
    [SerializeField] private float m_downwardSpringForce;

    public event Action OnTargetShot;

    [Button]
    public void MoveTargetDown()
    {
        JointSpring hingeSpring = m_hingeJoint.spring;
        hingeSpring.spring = m_downwardSpringForce;
        m_hingeJoint.spring = hingeSpring;
    }

    [Button]
    public void MoveTargetUp()
    {
        JointSpring hingeSpring = m_hingeJoint.spring;
        hingeSpring.spring = 0;
        m_hingeJoint.spring = hingeSpring;
    }

}
