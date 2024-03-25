using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable), typeof(AudioSource))]
public class DamageableSoundController : MonoBehaviour
{
    [SerializeField] private Damageable m_damageable;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private List<AudioClip> m_hitSounds;
    [SerializeField] private List<AudioClip> m_deathSounds;

    // Start is called before the first frame update
    void Start()
    {
        if (m_damageable != null && m_audioSource != null)
        {
            m_damageable.OnHit += PlayHitSound;
            m_damageable.OnDeath += PlayDeathSound;
        }
    }
    private void PlayHitSound(int _)
    {
        if (m_hitSounds.Count > 0)
        {
            m_audioSource.PlayOneShot(m_hitSounds[UnityEngine.Random.Range(0, m_hitSounds.Count)]);
        }
    }

    private void PlayDeathSound()
    {
        if (m_hitSounds.Count > 0)
        {
            m_audioSource.PlayOneShot(m_deathSounds[UnityEngine.Random.Range(0, m_deathSounds.Count)]);
        }
    }


}
