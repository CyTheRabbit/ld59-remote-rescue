using UnityEngine;

namespace Lag
{
    public class Projectile : MonoBehaviour
    {
        public Vector2 Direction;
        public float Speed;
        public float Lifetime;

        public void Update()
        {
            transform.position += (Vector3)(Speed * Time.deltaTime * Direction);
            Lifetime -= Time.deltaTime;
            if (Lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}