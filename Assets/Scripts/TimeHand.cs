using UnityEngine;

public class TimeHand : MonoBehaviour
{
    [SerializeField] private float delaySeconds = 1;

    private void Start()
    {
        InvokeRepeating(nameof(TickForward), 0, delaySeconds);
    }

    // Update is called once per frame
    private void TickForward()
    {
        gameObject.transform.Rotate(0, 360f/6f, 0);
        EventManager.ClockHandMoved();
    }
}