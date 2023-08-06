using System.Collections;
using UnityEngine;

public class ClockHand : MonoBehaviour
{
    [SerializeField] private float tickRotationDegrees = 360f / 60f;
    private int currentTick = 0;

    private const float RotationDuration = 0.03f;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private float lerpTime;
    
    public void TickForward()
    {
        RotateByDegrees(tickRotationDegrees);
        currentTick++;
    }

    public void TickBackward()
    {
        RotateByDegrees(-tickRotationDegrees);
        currentTick--;
    }

    private void RotateByDegrees(float degrees)
    {
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, degrees, 0));
        initialRotation = transform.rotation;
        lerpTime = 0f;
        StartCoroutine(InterpolateRotation());
    }

    private IEnumerator InterpolateRotation()
    {
        while (lerpTime < 1f)
        {
            lerpTime += Time.deltaTime / RotationDuration;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, lerpTime);
            yield return null;
        }
    }
    
    public float GetCurrentTick()
    {
        return currentTick;
    }
}