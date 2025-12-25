using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAimOffset : MonoBehaviour
{
    [Header("Positions")]
    public Vector3 hipPosition;
    public Vector3 hipRotation;
    public Vector3 adsPosition;
    public Vector3 adsRotation;

    public float lerpSpeed = 12f;
    bool isAiming;

    void Start()
    {
        // Use whatever pose is set in editor as hip pose
        hipPosition = transform.localPosition;
        hipRotation = transform.localEulerAngles;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        isAiming = Mouse.current.rightButton.isPressed;

        Vector3 targetPos = isAiming ? adsPosition : hipPosition;
        Vector3 targetRot = isAiming ? adsRotation : hipRotation;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            lerpSpeed * Time.deltaTime
        );

        Vector3 currentRot = transform.localEulerAngles;
        currentRot.x = Mathf.LerpAngle(currentRot.x, targetRot.x, lerpSpeed * Time.deltaTime);
        currentRot.y = Mathf.LerpAngle(currentRot.y, targetRot.y, lerpSpeed * Time.deltaTime);
        currentRot.z = Mathf.LerpAngle(currentRot.z, targetRot.z, lerpSpeed * Time.deltaTime);
        transform.localEulerAngles = currentRot;
    }
}
