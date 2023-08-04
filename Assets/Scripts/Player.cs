using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.transform.Rotate(0, 360f/60f, 0);
            EventManager.PlayerMoved();
        }
    }
}