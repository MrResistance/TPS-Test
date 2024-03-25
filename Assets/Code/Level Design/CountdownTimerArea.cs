using UnityEditor;
using UnityEngine;

public class CountdownTimerArea : MonoBehaviour
{
    [SerializeField] private countType m_countType;
    private enum countType { timer, countdown };

    [SerializeField] private areaType m_areaType;
    private enum areaType { start, end, other }

    [SerializeField] private bool m_resetOnRestart = true;

    private void OnTriggerEnter(Collider other)
    {
        switch (m_areaType)
        {
            case areaType.start:
                switch (m_countType)
                {
                    case countType.timer:
                        ScreenspaceUIManager.Instance.CountdownTimer.StartTimer(true);
                        break;
                    case countType.countdown:
                        ScreenspaceUIManager.Instance.CountdownTimer.StartCountdown(0.0f);
                        break;
                    default:
                        break;
                }
                break;
            case areaType.end:
                switch (m_countType)
                {
                    case countType.timer:
                        ScreenspaceUIManager.Instance.CountdownTimer.StopTimer();
                        break;
                    case countType.countdown:
                        ScreenspaceUIManager.Instance.CountdownTimer.StopCountdown();
                        break;
                    default:
                        break;
                }
                break;
            case areaType.other:
                break;
            default:
                break;
        }
    }
}
