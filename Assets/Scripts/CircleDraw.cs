using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleDraw : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private float notchOffset = 0f;
    [SerializeField] private float segmentDegrees = 360f / 60f;
        
    [SerializeField] private List<int> notchNthSegment = new();
    

    private LineRenderer lineRenderer;
    
    private GameObject startTarget;
    private GameObject endTarget;


    public void Draw()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        
        // calculate points count - one point every 60th of a circle
        int pointsCount = Mathf.RoundToInt(360 / segmentDegrees);
        lineRenderer.enabled = pointsCount >= 2;
        if (pointsCount < 2)
            return;

        List<Vector3> arcPoints = CalculateArcPoints(pointsCount);
        lineRenderer.positionCount = arcPoints.Count;

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

            if (notchOffset != 0)
            {
                float notchRadius = radius;

                if (!notchNthSegment.Count.Equals(0))
                {
                    notchRadius += notchNthSegment.Where(nthSegment => i % nthSegment == 0).Sum(nthSegment => notchOffset);

                    if (Math.Abs(notchRadius - radius) > 0.01)
                    {
                        float notchX = notchRadius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);
                        float notchY = notchRadius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
                        points.Add(new Vector3(notchX, 0f, notchY));
                        points.Add(new Vector3(x, 0f, z));
                    }
                }

            }

            currentAngle += angleStep;
        }
            
        // last point closes the circle
        points.Add(points[0]);
        return points;
    }
}