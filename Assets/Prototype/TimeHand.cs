using UnityEngine;

namespace Prototype
{
    public class TimeHand : MonoBehaviour
    {
        [SerializeField] private float delaySeconds = 1;
        [SerializeField] private ArcDraw arcDrawer;

        private void Start()
        {
            InvokeRepeating(nameof(TickForward), 0, delaySeconds);
        }

        // Update is called once per frame
        private void TickForward()
        {
            gameObject.transform.Rotate(0, 360/60, 0);
            arcDrawer.Draw();
        }
    }
}
