using UnityEngine;

namespace Lag
{
    public class Bot : MonoBehaviour
    {
        private static readonly int ShootHash = Animator.StringToHash("Shoot");
        private static readonly int DirectionXHash = Animator.StringToHash("DirectionX");
        private static readonly int DirectionYHash = Animator.StringToHash("DirectionY");
        private static readonly int MoveHash = Animator.StringToHash("Move");

        public float Speed;
        public float Acceleration;
        public float ShootCooldown;
        public float Knockback;
        public Projectile ProjectilePrefab;
        public Animator Animator;
        public Rigidbody2D Rigidbody;

        public LaggyInput.Signal Signal;
        private bool fireBuffered;
        private float lastShotTime;
        private Vector2 velocity;

        public void Update()
        {
            velocity = Vector2.MoveTowards(
                velocity,
                Signal.Move ? Signal.Direction * Speed : Vector2.zero,
                Acceleration * Time.deltaTime);

            // transform.position += (Vector3)(velocity * Time.deltaTime);
            Rigidbody.position += velocity * Time.deltaTime;

            if ((Signal.Fire || fireBuffered) && Time.time > lastShotTime + ShootCooldown)
            {
                Shoot();
                Animator.SetTrigger(ShootHash);
                fireBuffered = false;
            }
        }

        public void Receive(LaggyInput.Signal newSignal)
        {
            Signal = newSignal;
            if (Signal.Fire)
            {
                fireBuffered = true;
            }

            Animator.SetFloat(DirectionXHash, Signal.Direction.x);
            Animator.SetFloat(DirectionYHash, Signal.Direction.y);
            Animator.SetBool(MoveHash, Signal.Move);
        }

        private void Shoot()
        {
            var projectile = Instantiate(ProjectilePrefab);
            projectile.transform.position = transform.position;
            projectile.Direction = Signal.Direction;
            projectile.gameObject.SetActive(true);
            lastShotTime = Time.time;
            velocity += -Knockback * Signal.Direction;
        }
    }
}