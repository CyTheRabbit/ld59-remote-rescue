using System.Collections.Generic;
using UnityEngine;

namespace Lag
{
    public class EnemySpawner : MonoBehaviour
    {
        public PawnBase Prefab;
        public int OverallLimit = 1;
        public int SimultaneousLimit = 1;

        private int totalSpawned = 0;
        private readonly List<PawnBase> instantiatedPawns = new();

        public void Spawn()
        {
            if (OverallLimit > 0 && totalSpawned >= OverallLimit) { return; }
            instantiatedPawns.RemoveAll(pawn => !pawn.isActiveAndEnabled);
            if (instantiatedPawns.Count >= SimultaneousLimit) { return; }

            var instance = Instantiate(Prefab, transform.position, Quaternion.identity);
            instantiatedPawns.Add(instance);
            totalSpawned++;
        }
    }
}