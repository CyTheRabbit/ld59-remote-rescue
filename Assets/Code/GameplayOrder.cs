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

        public Camera LaggyCamera;
        public MusicMixer MusicMixer;
        public Bot Bot;
        public SignalStrengthIndicator SignalStrengthIndicator;
        public RadioIndicator RadioIndicator;
        public Volume PoorConnectionVolume;

        private SignalSource[] signalSources;
        private int framesSinceLastRedraw;

        public void Start()
        {
            signalSources = FindObjectsByType<SignalSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Application.targetFrameRate = 60;
        }

        public void Update()
        {
            var signalStrength = GetSignalStrength();
            var lag = signalStrength >= LagGradation.Length ? LagGradation[^1] : LagGradation[signalStrength];
            SignalStrengthIndicator.SetSignalStrength(signalStrength);

            laggyInput.ScanFrameInputs();
            RadioIndicator.DisplaySignals(laggyInput, lag);
            while (laggyInput.ReceiveNextSignal(lag) is { } signal)
            {
                Bot.SetInput(signal);
            }
            PoorConnectionVolume.weight = Mathf.InverseLerp(4, 12, signalStrength);

            var framesToRedraw = signalStrength;
            framesSinceLastRedraw++;
            if (framesSinceLastRedraw > framesToRedraw)
            {
                LaggyCamera.Render();
                framesSinceLastRedraw = 0;
            }
            MusicMixer.SetSignalStrength(signalStrength);
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