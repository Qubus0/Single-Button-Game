using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstaclePrefab;
    // delay range in seconds
    [SerializeField] private Vector2 delayRange = new(1, 3);
        
    [SerializeField] private float minDistanceDegrees = 30;
    [SerializeField] private float arcLengthDegrees = 90;

    private void Start()
    {
        SpawnObstacle();    
    }

    private void SpawnObstacle()
    {
        Invoke(nameof(SpawnObstacle), Random.Range(delayRange.x, delayRange.y));
            
        if (GameObject.FindGameObjectsWithTag("Obstacle").Length >= 2)
            return;
            
        // new rotation starting from the player up to the arc length in steps of 6 degrees (1/60th of a circle)
        float rotationFromPlayer = player.transform.rotation.eulerAngles.y;
            
        int newRotation = (int) Random.Range(0, arcLengthDegrees / 6) * 6;
        gameObject.transform.Rotate(0, rotationFromPlayer + minDistanceDegrees + newRotation, 0);
            
        Instantiate(
            obstaclePrefab, 
            spawnPosition.transform.position + obstaclePrefab.transform.position, 
            spawnPosition.transform.rotation
        );
            
        // reset rotation
        gameObject.transform.rotation = Quaternion.identity;
    }
}