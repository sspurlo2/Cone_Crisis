using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform CameraPosition;

    void Start()
    {
        // optional: check for null here if you want
    }

    void Update()
    {
        transform.position = CameraPosition.position;
    }
}
