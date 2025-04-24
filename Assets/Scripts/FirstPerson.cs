using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public Transform orientation;

    private float XRotation;
    private float YRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        YRotation += mouseX;
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);
        // this line above this comment basically makes it you can't rotate your camera 360 degrees up and down (your head can do that silly goose)


        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        orientation.rotation = Quaternion.Euler(0, YRotation, 0);
    }
}
