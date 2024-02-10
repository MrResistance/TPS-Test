using UnityEngine;

public class DamageableChild : MonoBehaviour
{
    [SerializeField] private Damageable m_damageable;
    [SerializeField] private int m_damageModifier = 1;
    public void LoseHitPoints(int damage)
    {
        m_damageable.LoseHitPoints(damage * m_damageModifier);
    }
    public void LoseHitPoints(int damage, Vector3 hitPosition)
    {
        m_damageable.LoseHitPoints(damage * m_damageModifier);
        m_damageable.HitPosition = hitPosition;
    }
}
