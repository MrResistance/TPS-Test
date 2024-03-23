using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private Interactable m_interactable;
    private LayerMask m_layerMask;

    private void Start()
    {
        m_layerMask = GameSettings.Instance.InteractableLayer;
        PlayerInputs.Instance.OnInteractPressed += Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((m_layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            m_interactable = other.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((m_layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            m_interactable = null;
        }
    }

    private void Interact()
    {
        m_interactable?.OnInteract();
    }
}
