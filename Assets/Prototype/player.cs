using UnityEngine;

namespace Prototype
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ArcDraw arcDrawer;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.transform.Rotate(0, 360f/60f, 0);
                EventManager.PlayerMoved();
                arcDrawer.Draw();
            }
        }
    }
}
