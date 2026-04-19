using Lag;
using UnityEngine;

namespace Enemies
{
    public class Bat : PawnBase
    {
        private GameObject player;

        public new void Start()
        {
            base.Start();
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        public new void FixedUpdate()
        {
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