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
        currentTick++;
        RotateToCurrentTick();
    }

    public void TickBackward()
    {
        currentTick--;
        RotateToCurrentTick();
    }

    private void RotateToCurrentTick()
    {
        targetRotation = Quaternion.Euler(new Vector3(0, tickRotationDegrees * currentTick, 0));
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