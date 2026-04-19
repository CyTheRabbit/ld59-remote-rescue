using Lag;
using UnityEngine;

namespace Enemies
{
    public class Bat : PawnBase
    {
        public new void FixedUpdate()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            SetInput(new PawnInput
            {
                Time = Time.time,
                Direction = (player.transform.position - transform.position).normalized,
                Fire = false,
                Move = true,
            });

            base.FixedUpdate();
        }
    }
}