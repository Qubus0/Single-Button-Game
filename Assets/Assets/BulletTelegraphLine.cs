using System.Collections;
using UnityEngine;

namespace Assets
{
    public class BulletTelegraphLine : MonoBehaviour
    {
        public float MaxLifetime { private get; set; } = 1f;
        private float lifetime;
        private float blinkTime;
        private LineRenderer lineRenderer;

        private State state;
        private float lastBlinkTime;

        private enum State
        {
            Default,
            Blinking,
            Static,
        }

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            lifetime = MaxLifetime;
            blinkTime = MaxLifetime/2;
        }

        private void Update()
        {
            // reduce the lifetime of the line
            lifetime -= Time.deltaTime;

            if (lifetime <= blinkTime && state == State.Default)
                StartCoroutine(BlinkCoroutine());

            if (lifetime < 0 && state == State.Blinking)
            {
                state = State.Static;
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
                
                var oldPosition = lineRenderer.GetPosition(1);
                var globalPosition = transform.TransformPoint(oldPosition);
                lineRenderer.useWorldSpace = true;
                lineRenderer.SetPosition(1, globalPosition);
            }
            
            if (lifetime <= -blinkTime)
                Destroy(gameObject);
        }

        private IEnumerator BlinkCoroutine()
        {
            state = State.Blinking;
            while (state == State.Blinking)
            {
                // Toggle the visibility of the line renderer
                lineRenderer.enabled = !lineRenderer.enabled;
                if (lifetime > blinkTime / 2)
                    yield return new WaitForSeconds(blinkTime/4);
                else
                    yield return new WaitForSeconds(blinkTime/8);
            }
        }
    }
}