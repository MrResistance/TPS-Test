using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [Tooltip("When set to true, when reloading, the player loses the rest of what's in their current clip." +
        "\n\nWhen set to false, when reloading, whatever's left in their current clip moves to their reserve ammo.")]
    public bool RealisticReloadingAmmoCount = false;

    public LayerMask DamageableLayer;
    public LayerMask BulletImpactDecalLayer;
    public LayerMask InteractableLayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
