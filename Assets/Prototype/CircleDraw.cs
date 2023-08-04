using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prototype
{
    public class CircleDraw : MonoBehaviour
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private float notchOffset = -0.1f;

        private LineRenderer lineRenderer;
    
        private GameObject startTarget;
        private GameObject endTarget;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
                lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        private void Start()
        {
            Draw();
        }

        private void Draw()
        {
            // calculate points count - one point every 60th of a circle
            int pointsCount = Mathf.RoundToInt(360 / (360f / 60f));
            lineRenderer.enabled = pointsCount >= 2;
            if (pointsCount < 2)
                return;

            List<Vector3> arcPoints = CalculateArcPoints(pointsCount);
            lineRenderer.positionCount = arcPoints.Count +1;

            for (int i = 0; i <= arcPoints.Count; i++)
                lineRenderer.SetPosition(i, arcPoints[i]);
        }
    
        private List<Vector3> CalculateArcPoints(int pointsCount)
        {
            var points = new List<Vector3>();
            float angleStep = 360f / pointsCount;
            float currentAngle = 0;

            for (int i = 0; i <= pointsCount; i++)
            {
                float x = radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);
                float z = radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
                points.Add(new Vector3(x, 0f, z));
                
                float notchRadius = radius + notchOffset;
                if (i % 5 == 0)
                    notchRadius += notchOffset;
                if (i % 15 == 0)
                    notchRadius += notchOffset;
                float notchX = notchRadius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);
                float notchY = notchRadius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
                points.Add(new Vector3(notchX, 0f, notchY));
                
                points.Add(new Vector3(x, 0f, z));
                currentAngle += angleStep;
            }
            
            return points;
        }
    }
}