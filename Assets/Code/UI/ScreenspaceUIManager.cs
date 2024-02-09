using UnityEngine;
using UnityEngine.UI;

public class ScreenspaceUIManager : MonoBehaviour
{
    public static ScreenspaceUIManager Instance;

    [SerializeField] private Image m_crosshair;

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

    private void Start()
    {
        if (PlayerInputs.Instance == null)
        {
            return;
        }

        PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
        PlayerInputs.Instance.OnSecondaryPressed += EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased += DisableCrosshair;
    }

    #region Subscriptions

    private void OnEnable()
    {
        if (PlayerInputs.Instance == null)
        {
            return;
        }

        PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
        PlayerInputs.Instance.OnSecondaryPressed += EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased += DisableCrosshair;
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
    }

    private void OnDestroy()
    {
        PlayerInputs.Instance.OnSecondaryPressed -= EnableCrosshair;
        PlayerInputs.Instance.OnSecondaryReleased -= DisableCrosshair;
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
}
