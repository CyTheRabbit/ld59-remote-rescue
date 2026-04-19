using UnityEngine;

namespace Lag
{
    public class Projectile : MonoBehaviour
    {
        public Vector2 Direction;
        public float Speed;
        public float Lifetime;
        public LayerMask LayerMask;

        public void FixedUpdate()
        {
            if (Physics2D.Raycast(transform.position, Direction, Time.deltaTime * Speed, LayerMask))
            {
                Lifetime = 0;
            }

            var travel = Speed * Time.deltaTime * Direction;
            transform.position += (Vector3)travel;
            Lifetime -= Time.deltaTime;
            if (Lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}