using UnityEngine;

namespace Lag
{
    public class TrapZone : MonoBehaviour
    {
        public float RespawnCooldown = 20;

        private float respawnTime;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (respawnTime > Time.time) { return; }
            if (!other.gameObject.CompareTag("Player")) { return; }

            respawnTime = Time.time + RespawnCooldown;
            foreach (var spawner in GetComponentsInChildren<EnemySpawner>())
            {
                spawner.Spawn();
            }
        }
    }
}