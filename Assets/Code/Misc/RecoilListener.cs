using UnityEngine;

public class RecoilListener : ObjectToScreenCenter
{
    public static RecoilListener Instance;

    public Vector2 Recoil;

    private Vector3 m_localStartPosition;
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
    public override void Update()
    {
        m_localStartPosition = transform.localPosition;

        if (Recoil == Vector2.zero)
        {
            base.Update();
            return;
        }

        transform.position += new Vector3(Recoil.x, Recoil.y, m_localStartPosition.z);

        transform.position = new Vector3(
        Mathf.Lerp(transform.position.x, m_localStartPosition.x, Time.deltaTime * 50),
        Mathf.Lerp(transform.position.y, m_localStartPosition.y, Time.deltaTime * 50), m_localStartPosition.z);
    }
}
