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
        public struct Signal : IEquatable<Signal>
        {
            public float Time;
            public Vector2 Direction;
            public bool Move;
            public bool Fire;

            public bool Equals(Signal other) =>
                Direction == other.Direction
                && Move == other.Move
                && Fire == other.Fire;

            public override bool Equals(object obj) => obj is Signal other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(Direction, Move, Fire);

            public static bool operator ==(Signal left, Signal right) => left.Equals(right);

            public static bool operator !=(Signal left, Signal right) => !left.Equals(right);
        }

        public readonly List<Signal> SignalQueue = new(capacity: 32);
        public Signal LastSignal;

        public Signal? ReceiveNextSignal(float lag)
        {
            var time = Time.time - lag;
            if (!SignalQueue.Any()) { return null; }
            var signal = SignalQueue[0];
            if (signal.Time > time) { return null; }
            SignalQueue.RemoveAt(0);
            return signal;
        }

        public void ScanFrameInputs()
        {
            var keyboard = Keyboard.current;
            var signal = new Signal { Time = Time.time };
            TryChangeDirection(keyboard.wKey, Vector2.up);
            TryChangeDirection(keyboard.aKey, Vector2.left);
            TryChangeDirection(keyboard.sKey, Vector2.down);
            TryChangeDirection(keyboard.dKey, Vector2.right);
            TryAttack(keyboard.spaceKey);
            if (signal.Direction == Vector2.zero)
            {
                signal.Direction = LastSignal.Direction;
            }
            if (signal != LastSignal)
            {
                LastSignal = signal;
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