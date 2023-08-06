using UnityEngine;

public class ArcDraw : MonoBehaviour
{
    private const float Radius = 5f;

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
        // if the angle is negative, flip start and end
        if (angle < 0)
        {
            (startAngle, endAngle) = (endAngle, startAngle);
            angle = -angle;
        }
        
        // calculate points count - one point every 60th of a circle
        int pointsCount = Mathf.RoundToInt(angle / (360f / 60f));
        lineRenderer.enabled = pointsCount >= 2;
        if (pointsCount < 2)
            return;

        var color = pointsCount switch
        {
            >= (360 / 60) * 270 => Color.red,
            >= (360 / 60) * 135 => new Color(1f, 0.5f, 0f),
            >= (360 / 60) * 45 => Color.yellow,
            _ => new Color(1f, 1f, .7f)
        };

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
        float totalAngle = (endAngle - startAngle) % 360f;
        float angleStep = totalAngle / pointsCount;
        float currentAngle = startAngle;

        for (int i = 0; i <= pointsCount; i++)
        {
            float x = Radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);
            float z = Radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
            points[i] = new Vector3(x, 0f, z);
            currentAngle += angleStep;
        }

        return points;
    }
}