using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Lag
{
    public class LaggyInput
    {
        public readonly List<PawnInput> SignalQueue = new(capacity: 32);
        public PawnInput LaggySignal;
        public PawnInput RealtimeSignal;

        public PawnInput? ReceiveNextSignal(float lag)
        {
            var time = Time.time - lag;
            if (!SignalQueue.Any()) { return null; }
            var signal = SignalQueue[0];
            if (signal.Time > time) { return null; }
            SignalQueue.RemoveAt(0);
            LaggySignal = signal;
            return signal;
        }

        public void ScanFrameInputs()
        {
            var keyboard = Keyboard.current;
            var signal = new PawnInput { Time = Time.time };
            TryChangeDirection(keyboard.wKey, Vector2.up);
            TryChangeDirection(keyboard.aKey, Vector2.left);
            TryChangeDirection(keyboard.sKey, Vector2.down);
            TryChangeDirection(keyboard.dKey, Vector2.right);
            TryAttack(keyboard.spaceKey);
            if (signal.Direction == Vector2.zero)
            {
                signal.Direction = RealtimeSignal.Direction;
            }
            if (signal != RealtimeSignal)
            {
                RealtimeSignal = signal;
                SignalQueue.Add(signal);
            }

            void TryChangeDirection(KeyControl key, Vector2 direction)
            {
                if (key.isPressed)
                {
                    signal.Direction += direction;
                    signal.Move = true;
                }
            }

            void TryAttack(KeyControl key)
            {
                signal.Fire = key.isPressed;
            }
        }
    }
}