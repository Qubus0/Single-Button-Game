using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArcDraw : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [FormerlySerializedAs("arcSegmentLength")] public float segmentDegrees = 360f/60f;

    [SerializeField] private List<float> colorAngles = new();
    [SerializeField] private List<Color> colors = new();

    private LineRenderer lineRenderer;
    

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

    }

    public void Draw(float startAngle, float endAngle)
    {
        float angle = endAngle - startAngle;
        
        // calculate points count - one point every 60th of a circle
        int pointsCount = Mathf.RoundToInt(angle / segmentDegrees);
        lineRenderer.enabled = pointsCount >= 2;
        if (pointsCount < 2)
            return;

        var color = Color.white;
        for (int i = 0; i < colorAngles.Count; i++)
        {
            if (pointsCount >= colorAngles[i] / segmentDegrees)
            {
                color = colors[i];
                break;
            }
        }

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.positionCount = pointsCount + 1;
        Vector3[] arcPoints = CalculateArcPoints(startAngle, endAngle, pointsCount);

        for (int i = 0; i <= pointsCount; i++)
            lineRenderer.SetPosition(i, arcPoints[i]);
    }
    
    private Vector3[] CalculateArcPoints(float startAngle, float endAngle, int pointsCount)
    {
        var points = new Vector3[pointsCount + 1];
        float totalAngle = (endAngle - startAngle);
        float angleStep = totalAngle / pointsCount;
        float currentAngle = startAngle;

        for (int i = 0; i <= pointsCount; i++)
        {
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);
            float z = radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            points[i] = new Vector3(x, 0f, z);
            currentAngle += angleStep;
        }

        return points;
    }
}