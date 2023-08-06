using UnityEngine;

public class ClockHand : MonoBehaviour
{
    [SerializeField] private float tickRotationDegrees = 360f / 60f;
    private int currentTick = 0;

    public void TickForward()
    {
        gameObject.transform.Rotate(0, tickRotationDegrees, 0);
        currentTick++;
    }
    
    public void TickBackward()
    {
        gameObject.transform.Rotate(0, -tickRotationDegrees, 0);
        currentTick--;
    }
    
    public float GetCurrentTick()
    {
        return currentTick;
    }
}