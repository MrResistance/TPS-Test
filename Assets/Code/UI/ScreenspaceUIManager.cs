using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenspaceUIManager : MonoBehaviour
{
    public static ScreenspaceUIManager Instance;

    public CountdownTimer CountdownTimer;
    [SerializeField] private Image m_crosshair;
    [SerializeField] private TextMeshProUGUI m_ammoCounterText;
    [SerializeField] private TextMeshProUGUI m_interactText;

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
    
    #region Subscriptions
    private void Start()
    {
        if (PlayerInputs.Instance != null)
        {
            PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
            PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
            PlayerInputs.Instance.OnSecondaryPressed += EnableCrosshair;
            PlayerInputs.Instance.OnSecondaryReleased += DisableCrosshair;
        }

        if (WeaponRig.Instance != null)
        {
            WeaponRig.Instance.UpdateAmmoCounter -= UpdateAmmoCounterText;
            WeaponRig.Instance.UpdateAmmoCounter += UpdateAmmoCounterText;
        }
    }

    private void OnEnable()
    {
        if (PlayerInputs.Instance != null)
        {
            PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
            PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
            PlayerInputs.Instance.OnSecondaryPressed += EnableCrosshair;
            PlayerInputs.Instance.OnSecondaryReleased += DisableCrosshair;
        }

        if (WeaponRig.Instance != null)
        {
            WeaponRig.Instance.UpdateAmmoCounter -= UpdateAmmoCounterText;
            WeaponRig.Instance.UpdateAmmoCounter += UpdateAmmoCounterText;
        }
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
        WeaponRig.Instance.UpdateAmmoCounter -= UpdateAmmoCounterText;
    }

    private void OnDestroy()
    {
        PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
        WeaponRig.Instance.UpdateAmmoCounter -= UpdateAmmoCounterText;
    }
    #endregion

    public void EnableCrosshair()
    {
        m_crosshair.enabled = true;
    }

    public void DisableCrosshair() 
    {
        m_crosshair.enabled = false;
    }

    public void UpdateAmmoCounterText(int currentClip, int reserveAmmo)
    {
        m_ammoCounterText.text = currentClip.ToString() + " / " + reserveAmmo.ToString();
    }

    public void ClearAmmoCounterText()
    {
        m_ammoCounterText.text = "";
    }

    public void UpdateInteractText(string text)
    {
        m_interactText.text = text;
    }

    public void ClearInteractText()
    {
        m_interactText.text = "";
    }
}
