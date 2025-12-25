using UnityEngine;
using UnityEngine.InputSystem;   // NEW

public class CameraLook : MonoBehaviour
{
    public float sensitivity = 50;      // Mouse sensitivity
    public float minYaw = -90;          // Left limit
    public float maxYaw = 90;           // Right limit
    public float minPitch = -10f;         // Look down limit
    public float maxPitch = 45f;          // Look up limit

    private float yaw;    // horizontal rotation
    private float pitch;  // vertical rotation

    void Start()
    {
        Vector3 euler = transform.localEulerAngles;
        yaw = euler.y;
        pitch = euler.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
{
    if (Mouse.current == null)
        return;

    Vector2 delta = Mouse.current.delta.ReadValue();

    float mouseX = delta.x * sensitivity * Time.deltaTime;
    float mouseY = delta.y * sensitivity * Time.deltaTime;

    yaw   += mouseX;
    pitch -= mouseY;

    yaw   = Mathf.Clamp(yaw,   minYaw,   maxYaw);
    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

    transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
}

}
