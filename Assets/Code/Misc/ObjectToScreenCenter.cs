using UnityEngine;

public class ObjectToScreenCenter : MonoBehaviour
{
    public float distanceFromCamera = 10.0f; // Distance from the camera to the object

    void Update()
    {
        // Calculate the screen center point
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, distanceFromCamera);

        // Convert the screen point to a world point
        transform.position = Camera.main.ScreenToWorldPoint(screenCenter);
    }
}
