using TMPro;
using UnityEngine;

public class Scoreable : MonoBehaviour
{
    [SerializeField] private Damageable m_damageable;
    [SerializeField] private bool m_repeatedScoring = false;

    // Start is called before the first frame update
    void Start()
    {
        m_damageable.OnHit += ShowPoints;
    }

    private void ShowPoints(int amount)
    {
        Vector3 pointTextSpawnPoint = transform.position + new Vector3(0, 2, 0);

        var pointText = ObjectPooler.Instance.SpawnFromPool("PointText", pointTextSpawnPoint, transform.rotation);

        string pointTextFinal = "";

        if (amount > 0)
        {
            pointTextFinal = "+" + amount.ToString();
        }
        else
        {
            pointTextFinal = amount.ToString();
        }

        pointText.GetComponent<TextMeshProUGUI>().text = pointTextFinal.ToString();

        if (!m_repeatedScoring)
        {
            m_damageable.OnHit -= ShowPoints;
        }
    }
}
