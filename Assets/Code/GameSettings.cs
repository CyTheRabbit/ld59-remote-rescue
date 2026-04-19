using System;
using UnityEngine;

namespace Lag
{
    [Serializable]
    public struct SignalEffects
    {
        public string Name;
        public float Lag;
        public float PostProcessing;
        public int FrameLoss;
        public bool LowSignalWarning;
        public float MusicPitch;
        public float NoiseVolume;
    }

    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        public SignalEffects[] SignalEffects;

        public SignalEffects GetSignalEffects(int signalStrength) =>
            signalStrength >= SignalEffects.Length
                ? SignalEffects[^1]
                : SignalEffects[signalStrength];
    }
}