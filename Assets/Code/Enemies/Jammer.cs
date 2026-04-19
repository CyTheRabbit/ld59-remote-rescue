using Lag;
using UnityEngine;

namespace Enemies
{
    public class Jammer : PawnBase
    {
        public float DamagePanicTime;
        public float JamStart;
        public float JamStep;
        public int JamStrength;

        private float panicTime;

        public new void FixedUpdate()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (panicTime > 0)
            {
                var panicDirection = PickPanicDirection(player.transform.position);
                SetInput(new PawnInput
                {
                    Time = Time.time,
                    Direction = panicDirection,
                    Fire = false,
                    Move = true,
                });
            }
            else
            {
                var playerDirection = (player.transform.position - transform.position).normalized;
                SetInput(new PawnInput
                {
                    Time = Time.time,
                    Direction = playerDirection,
                    Fire = false,
                    Move = false,
                });
            }

            panicTime -= Time.deltaTime;
            base.FixedUpdate();
        }

        public override void Damage(Vector2 direction)
        {
            base.Damage(direction);
            panicTime = DamagePanicTime;
        }

        private Vector2 PickPanicDirection(Vector2 source)
        {
            var diff = (Vector2)transform.position - source;
            var primaryDirection = Mathf.Abs(diff.x) > Mathf.Abs(diff.y)
                ? Vector2.right * Mathf.Sign(diff.x)
                : Vector2.up * Mathf.Sign(diff.y);
            var secondaryDirection = Mathf.Abs(diff.x) <= Mathf.Abs(diff.y)
                ? Vector2.right * Mathf.Sign(diff.x)
                : Vector2.up * Mathf.Sign(diff.y);
            return (primaryDirection + secondaryDirection / 2).normalized;
        }

        public int GetJamStrengthAtPosition(Vector2 botPosition)
        {
            var distance = Vector2.Distance(transform.position, botPosition);
            return distance < JamStart
                ? JamStrength
                : JamStrength - Mathf.CeilToInt((distance - JamStart) / JamStep);
        }

        public void OnDrawGizmos()
        {
            for (var i = JamStrength - 1; i >= 0; i--)
            {
                Gizmos.color = Color.Lerp(Color.red, Color.yellow, (float)i / (JamStrength - 1));
                Gizmos.DrawWireSphere(transform.position, JamStart + JamStep * i);
            }
        }
    }
}