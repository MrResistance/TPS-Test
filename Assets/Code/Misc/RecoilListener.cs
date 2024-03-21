using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RecoilListener : MonoBehaviour
{
    public static RecoilListener Instance;

    public Vector2 Recoil;

    [SerializeField] private MultiAimConstraint m_aimConstraint;

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

    // Update is called once per frame
    public void Update()
    {
        if (WeaponRig.Instance.CurrentWeapon != null)
        {
            m_aimConstraint.data.offset = new Vector3(
                m_aimConstraint.data.offset.x, 
                Recoil.y * WeaponRig.Instance.CurrentWeapon.WeaponData.RecoilStrength,
                m_aimConstraint.data.offset.z);
        }
    }
}