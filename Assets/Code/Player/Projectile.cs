using UnityEngine;

namespace Lag
{
    public class Projectile : MonoBehaviour
    {
        public Vector2 Direction;
        public float Speed;
        public float Radius = 0.25f;
        public float Lifetime;
        public LayerMask LayerMask;

        public void FixedUpdate()
        {
            if (Lifetime > 0)
            {
                var hit = Physics2D.CircleCast(transform.position, Radius, Direction, Time.deltaTime * Speed, LayerMask);
                if (hit)
                {
                    Lifetime = 0;
                    var travel = (hit.distance + Radius) * Direction;
                    transform.position += (Vector3)travel;
                    if (hit.collider.GetComponentInParent<PawnBase>() is { } pawn)
                    {
                        pawn.Damage(Direction);
                    }
                }
                else
                {
                    var travel = Speed * Time.deltaTime * Direction;
                    transform.position += (Vector3)travel;
                }
            }

            Lifetime -= Time.deltaTime;
            if (Lifetime <= -0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}