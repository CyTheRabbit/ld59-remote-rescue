using System;
using UnityEngine;

namespace Lag
{
    public struct PawnInput : IEquatable<PawnInput>
    {
        public float Time;
        public Vector2 Direction;
        public bool Move;
        public bool Fire;

        public bool Equals(PawnInput other) =>
            Direction == other.Direction
            && Move == other.Move
            && Fire == other.Fire;

        public override bool Equals(object obj) => obj is PawnInput other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Direction, Move, Fire);

        public static bool operator ==(PawnInput left, PawnInput right) => left.Equals(right);

        public static bool operator !=(PawnInput left, PawnInput right) => !left.Equals(right);
    }
}