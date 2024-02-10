using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    [SerializeField] private Weapon m_weapon;
    [SerializeField] private bool m_rayFromBarrel = false;
    #region Event Subscriptions
    private void Start()
    {
        m_weapon.OnShoot += HitCalculation;
    }
    private void OnEnable()
    {
        m_weapon.OnShoot -= HitCalculation;
        m_weapon.OnShoot += HitCalculation;
    }
    private void OnDisable()
    {
        m_weapon.OnShoot -= HitCalculation;
    }
    private void OnDestroy()
    {
        m_weapon.OnShoot -= HitCalculation;
    }
    #endregion

    private void HitCalculation()
    {
        Ray ray;
        if (!m_rayFromBarrel)
        {
            // Get the center point of the screen
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            // Create a ray from the center of the screen
            ray = Camera.main.ScreenPointToRay(screenCenter);
        }
        else
        {
            ray = new Ray(m_weapon.Barrel.position, m_weapon.transform.forward);
        }

        if (Physics.Raycast(ray, out m_weapon.m_raycastHit, m_weapon.m_effectiveRange, GameSettings.Instance.DamageableLayer))
        {
            if (m_weapon.m_raycastHit.collider.TryGetComponent<Rigidbody>(out _))
            {
                m_weapon.m_raycastHit.rigidbody.AddExplosionForce(m_weapon.m_hitForce, m_weapon.m_raycastHit.point, 1);
            }
            
            if (m_weapon.m_raycastHit.collider.TryGetComponent(out DamageableChild damageableChild))
            {
                damageableChild.LoseHitPoints(m_weapon.m_damage, damageableChild.transform.InverseTransformPoint(m_weapon.m_raycastHit.point));
                ObjectPooler.Instance.SpawnFromPool("BloodSplatterSmall", m_weapon.m_raycastHit.point, m_weapon.m_raycastHit.transform.rotation);
            }
            else if (m_weapon.m_raycastHit.collider.TryGetComponent(out Damageable damageable))
            {
                damageable.LoseHitPoints(m_weapon.m_damage);
                damageable.HitPosition = damageable.transform.InverseTransformPoint(m_weapon.m_raycastHit.point);
                ObjectPooler.Instance.SpawnFromPool("BloodSplatterSmall", m_weapon.m_raycastHit.point, m_weapon.m_raycastHit.transform.rotation);
            }
        }

        if (Physics.Raycast(ray, out m_weapon.m_raycastHit, m_weapon.m_effectiveRange, GameSettings.Instance.BulletImpactDecalLayer))
        {
            // Calculate the direction from the hit point back towards the ray origin
            Vector3 directionToHitPoint = m_weapon.m_raycastHit.point - ray.origin;
            directionToHitPoint.Normalize(); // Normalize to get just the direction

            // Calculate the rotation so that the forward vector of the decal points towards the hit point
            Quaternion rotationTowardsHitPoint = Quaternion.LookRotation(directionToHitPoint);

            // Spawn the BulletImpactDecal with the calculated rotation
            ObjectPooler.Instance.SpawnFromPool("BulletImpactDecal", m_weapon.m_raycastHit.point, rotationTowardsHitPoint);
        }

    }
}
