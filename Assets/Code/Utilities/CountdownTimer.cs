using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private countType m_countType;

    public enum countType { timer, countdown }

    private float m_currentTime;
    private float m_endTime;

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
        m_endTime = time;
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
        if (m_countdownActive || m_timerActive)
        {
            switch (m_countType)
            {
                case countType.timer:
                    TimerTick();
                    break;
                case countType.countdown:
                    CountdownTick();
                    break;
                default:
                    break;
            }
            UpdateText(m_currentTime);
        }
    }

    private void CountdownTick()
    {
        m_currentTime -= Time.deltaTime;
        if (m_currentTime <= m_endTime)
        {
            OnCountdownComplete?.Invoke();
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
