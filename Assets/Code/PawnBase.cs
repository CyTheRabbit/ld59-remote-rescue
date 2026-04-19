using System;
using UnityEngine;

namespace Lag
{
    public class PawnBase : MonoBehaviour
    {
        private static readonly int FireHash = Animator.StringToHash("Shoot");
        private static readonly int PerishHash = Animator.StringToHash("Perish");
        private static readonly int DirectionXHash = Animator.StringToHash("DirectionX");
        private static readonly int DirectionYHash = Animator.StringToHash("DirectionY");
        private static readonly int MoveHash = Animator.StringToHash("Move");

        public int MaxHealth;
        public float Speed;
        public float Acceleration;
        public float FireCooldown;
        public float FireKnockback;
        public float WallsKnockback;
        public float WallsStagger;
        public float DamageKnockback;
        public float DamageStagger;
        public Animator Animator;
        public Rigidbody2D Rigidbody;
        public Collider2D Collider;
        public LayerMask DamageLayers;

        public PawnInput Input;
        private bool fireBuffered;
        private float fireTime;
        private float staggerTime;
        private Vector2 velocity;

        [NonSerialized] public int Health;

        public void Start()
        {
            Health = MaxHealth;
        }

        private void OnEnable()
        {
            Rigidbody.WakeUp();
            Collider.enabled = true;
        }

        private void OnDisable()
        {
            Rigidbody.Sleep();
            Collider.enabled = false;
        }

        public void FixedUpdate()
        {
            staggerTime -= Time.deltaTime;
            fireTime -= Time.deltaTime;

            velocity = Vector2.MoveTowards(
                velocity,
                Input.Move && staggerTime <= 0 ? Input.Direction * Speed : Vector2.zero,
                Acceleration * Time.deltaTime);

            Rigidbody.MovePosition(Rigidbody.position + velocity * Time.deltaTime);

            if ((Input.Fire || fireBuffered) && fireTime <= 0)
            {
                fireTime = FireCooldown;
                fireBuffered = false;
                velocity += Input.Direction * -FireKnockback;

                Fire();

                Animator.SetTrigger(FireHash);
            }
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            var layerMask = 1 << other.gameObject.layer;
            var damages = (layerMask & DamageLayers) != 0;
            if (damages)
            {
                Damage(other.contacts[0].normal);
            }
            else
            {
                velocity = WallsKnockback * other.contacts[0].normal;
                staggerTime = WallsStagger;
            }
        }

        public void SetInput(PawnInput newInput)
        {
            Input = newInput;
            if (Input.Fire)
            {
                fireBuffered = true;
            }

            Animator.SetFloat(DirectionXHash, Input.Direction.x);
            Animator.SetFloat(DirectionYHash, Input.Direction.y);
            Animator.SetBool(MoveHash, Input.Move);
        }

        public virtual void Fire()
        {
        }

        public virtual void Damage(Vector2 direction)
        {
            velocity = DamageKnockback * direction;
            staggerTime = DamageStagger;
            Health--;
            if (Health <= 0)
            {
                Animator.SetTrigger(PerishHash);
                enabled = false;
            }
        }
    }
}