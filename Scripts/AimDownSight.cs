using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AimDownSight : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;
    public float hipFov = 60f;
    public float adsFov = 40f;
    public float fovLerpSpeed = 10f;

    [Header("Crosshair")]
    public RectTransform lineLeft;
    public RectTransform lineRight;
    public float hipSpacing = 15f;
    public float adsSpacing = 4f;
    public float crosshairLerpSpeed = 20f;

    bool isAiming;

    void Start()
    {
        if (mainCamera != null)
            mainCamera.fieldOfView = hipFov;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // Right mouse held = aim mode
        isAiming = Mouse.current.rightButton.isPressed;

        // Smooth FOV change
        float targetFov = isAiming ? adsFov : hipFov;
        mainCamera.fieldOfView = Mathf.Lerp(
            mainCamera.fieldOfView,
            targetFov,
            fovLerpSpeed * Time.deltaTime
        );

        // Smooth crosshair spacing
        float targetSpacing = isAiming ? adsSpacing : hipSpacing;

        if (lineLeft != null && lineRight != null)
        {
            Vector2 leftPos  = lineLeft.anchoredPosition;
            Vector2 rightPos = lineRight.anchoredPosition;

            leftPos.x  = Mathf.Lerp(leftPos.x,  -targetSpacing, crosshairLerpSpeed * Time.deltaTime);
            rightPos.x = Mathf.Lerp(rightPos.x,  targetSpacing, crosshairLerpSpeed * Time.deltaTime);

            lineLeft.anchoredPosition  = leftPos;
            lineRight.anchoredPosition = rightPos;
        }
    }
}
