using Sirenix.OdinInspector;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private float m_currentTime;

    private bool m_countdownActive;
    private bool m_timerActive;

    [SerializeField] private TextMeshProUGUI m_countdownTimerText;

    public event Action OnCountdownStarted;
    public event Action OnCountdownComplete;
    public event Action OnTimerStarted;

    public void StartCountdown(float time)
    {
        OnCountdownStarted?.Invoke();
        m_countdownActive = true;
        m_currentTime = time;
    }

    public void StopCountdown()
    {
        m_countdownActive = false;
    }

    public void StartTimer(bool reset)
    {
        OnTimerStarted?.Invoke();
        m_timerActive = true;
        if (reset)
        {
            m_currentTime = 0;
        }
    }

    public void StopTimer()
    {
        m_timerActive = false;
    }

    private void Update()
    {
        if (m_countdownActive)
        {
            CountdownTick();
            UpdateText(m_currentTime);
        }
        else if (m_timerActive)
        {
            TimerTick();
            UpdateText(m_currentTime);
        }
        
    }

    private void CountdownTick()
    {
        m_currentTime -= Time.deltaTime;
        if (m_currentTime <= 0)
        {
            OnCountdownComplete?.Invoke();
            m_countdownActive = false;
            m_currentTime = 0;
            UpdateText(m_currentTime);
        }
    }

    private void TimerTick()
    {
        m_currentTime += Time.deltaTime;
    }

    private void UpdateText(float time)
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int fractional = Mathf.FloorToInt((time * 100F) % 100F);

        // Format and display time
        m_countdownTimerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, fractional);


    }
}
