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

        private SignalSource[] signalSources;

        public void Start()
        {
            signalSources = FindObjectsByType<SignalSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        public void FixedUpdate()
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
            Application.targetFrameRate = (60 / Math.Max(1, signalStrength));
        }

        private int GetSignalStrength()
        {
            var botPosition = Bot.transform.position;
            var strength = 12;
            foreach (var source in signalSources)
            {
                if (!source.isActiveAndEnabled) { continue; }
                strength = Mathf.Min(strength, source.GetSignalStrengthAtPosition(botPosition));
            }
            return strength;
        }
    }
}