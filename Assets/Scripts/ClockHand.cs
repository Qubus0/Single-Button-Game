using UnityEngine;
using UnityEngine.Events;

public class ClockHand : MonoBehaviour
{
    [SerializeField] private float tickRotationDegrees = 360f / 60f;

    public void TickForward()
    {
        gameObject.transform.Rotate(0, tickRotationDegrees, 0);
    }
}