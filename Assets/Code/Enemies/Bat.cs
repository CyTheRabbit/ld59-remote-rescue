using Lag;
using UnityEngine;

namespace Enemies
{
    public class Bat : PawnBase
    {
        public new void FixedUpdate()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var diff = player.transform.position - transform.position;
            var direction = new Vector2(
                diff.x switch
                {
                    < -0.1f => -1,
                    > 0.1f => +1,
                    _ => 0
                },
                diff.y switch
                {
                    < -0.1f => -1,
                    > 0.1f => +1,
                    _ => 0
                });
            SetInput(new PawnInput
            {
                Time = Time.time,
                Direction = direction.normalized,
                Fire = false,
                Move = true,
            });

            base.FixedUpdate();
        }
    }
}