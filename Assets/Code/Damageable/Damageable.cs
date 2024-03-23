using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int m_maxHitPoints;
    [SerializeField] private int m_currentHitPoints;
    public int CurrentHitPoints => m_currentHitPoints;

    public event Action<int> OnHit;
    public event Action OnDeath;

    public Vector3 HitPosition;
    private void Start()
    {
        m_currentHitPoints = m_maxHitPoints;
    }

    public void SetMaxHitPoints(int amount)
    {
        m_maxHitPoints = amount;
    }

    public void GainHitPoints(int amount)
    {
        m_currentHitPoints += amount;
        if (m_currentHitPoints >= m_maxHitPoints)
        {
            m_currentHitPoints = m_maxHitPoints;
        }
    }

    public void LoseHitPoints(int amount)
    {
        m_currentHitPoints -= amount;
        if (m_currentHitPoints <= 0)
        {
            m_currentHitPoints = 0;
            OnDeath?.Invoke();
        }
        OnHit?.Invoke(amount);
    }
}
