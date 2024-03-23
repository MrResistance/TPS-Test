using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float Speed;
    public direction Direction;
    public enum direction { up, down, left, right, forward, back }
    private Vector3 m_direction;

    private void Start()
    {
        switch (Direction)
        {
            case direction.up:
                m_direction = Vector3.up;
                break;
            case direction.down:
                m_direction = Vector3.down;
                break;
            case direction.left:
                m_direction = Vector3.left;
                break;
            case direction.right:
                m_direction = Vector3.right;
                break;
            case direction.forward:
                m_direction = Vector3.forward;
                break;
            case direction.back:
                m_direction = Vector3.back;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_direction * Speed * Time.deltaTime);
    }
}
