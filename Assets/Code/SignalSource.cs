using UnityEngine;

namespace Lag
{
    public class SignalSource : MonoBehaviour
    {
        public float FalloffStart;
        public float FalloffStep;

        public int GetSignalStrengthAtPosition(Vector2 position)
        {
            var distance = Vector2.Distance(position, transform.position);
            return distance < FalloffStart ? 0 : Mathf.CeilToInt((distance - FalloffStart) / FalloffStep);
        }

        public void OnDrawGizmos()
        {
            for (var i = 11; i >= 0; i--)
            {
                var group = i / 3;
                Gizmos.color = Color.Lerp(Color.green, Color.red, group / 3f);
                Gizmos.DrawWireSphere(transform.position, FalloffStart + FalloffStep * i);
            }
        }
    }
}