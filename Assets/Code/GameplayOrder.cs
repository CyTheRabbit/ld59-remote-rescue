using System;
using Lag;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code
{
    public class GameplayOrder : MonoBehaviour
    {
        private readonly LaggyInput laggyInput = new();

        public float[] LagGradation;

        public Bot Bot;
        public SignalStrengthIndicator SignalStrengthIndicator;
        public RadioIndicator RadioIndicator;
        public Volume PoorConnectionVolume;

        public void Update()
        {
            var signalStrength = GetSignalStrength();
            var lag = signalStrength >= LagGradation.Length ? LagGradation[^1] : LagGradation[signalStrength];
            SignalStrengthIndicator.SetSignalStrength(signalStrength);

            laggyInput.ScanFrameInputs();
            RadioIndicator.DisplaySignals(laggyInput, Bot, lag);
            while (laggyInput.ReceiveNextSignal(lag) is { } signal)
            {
                Bot.Receive(signal);
            }
            PoorConnectionVolume.weight = Mathf.InverseLerp(4, 12, signalStrength);
            Application.targetFrameRate = Math.Clamp(60 - signalStrength * 5, 5, 60);
        }

        private int GetSignalStrength()
        {
            var distance = Bot.transform.position.magnitude;
            return Mathf.FloorToInt(distance / 5);
        }
    }
}