using UnityEngine;

namespace Assets
{
    public class Bullet : MonoBehaviour
    {
        public int Difficulty = 0;

        [SerializeField] private float damage = 1;
        [SerializeField] private float timeToLive = 5;
        [SerializeField] public float speed = 2;
    
        [SerializeField] public float telegraphTime = 1;
        [SerializeField] public BulletTelegraphLine telegraphLine;

        private void Awake()
        {
            if (telegraphLine == null)
                throw new System.Exception("Telegraph line is null");
        }

        private void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        
            // Rotate the cylinder on its side visually
            gameObject.transform.Rotate(90, 0, 0);
        }

        // Update is called once per frame
        private void Update()
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive <= 0)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            DealDamage(collision.gameObject);
            Destroy(gameObject);
        }

        private void DealDamage(GameObject otherGameObject)
        {
            // gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
